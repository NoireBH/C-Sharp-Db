namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Utilities;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGamesDto[] gameDtos = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

            HashSet<Game> validGames = new HashSet<Game>();
            HashSet<Developer> developers = new HashSet<Developer>();
            HashSet<Genre> genres = new HashSet<Genre>();
            HashSet<Tag> tags = new HashSet<Tag>();

            foreach (var gameDto in gameDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime releaseDate;
                bool isReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isReleaseDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (gameDto.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game g = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate
                };

                Developer gameDev = developers
                    .FirstOrDefault(d => d.Name == gameDto.Developer);

                if (gameDev == default)
                {
                    Developer newGameDev = new Developer()
                    {
                        Name = gameDto.Developer
                    };
                    developers.Add(newGameDev);

                    g.Developer = newGameDev;
                }
                else
                {
                    g.Developer = gameDev;
                }

                Genre gameGenre = genres
                    .FirstOrDefault(g => g.Name == gameDto.Genre);

                if (gameGenre == default)
                {
                    Genre newGenre = new Genre()
                    {
                        Name = gameDto.Genre
                    };

                    genres.Add(newGenre);
                    g.Genre = newGenre;
                }
                else
                {
                    g.Genre = gameGenre;
                }

                foreach (var tagName in gameDto.Tags)
                {
                    if (String.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }

                    Tag gameTag = tags
                        .FirstOrDefault(t => t.Name == tagName);

                    if (gameTag == default)
                    {
                        Tag newGameTag = new Tag()
                        {
                            Name = tagName
                        };

                        tags.Add(newGameTag);

                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = newGameTag
                        });
                    }
                    else
                    {
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = gameTag
                        });
                    }

                    if (g.GameTags.Count == 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                validGames.Add(g);
                sb.AppendLine(String.Format(SuccessfullyImportedGame, g.Name, g.Genre.Name, g.GameTags.Count));
            }

            context.Games.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportUserDto[] usersDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            var validUsers = new HashSet<User>();

            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var userCards = new HashSet<Card>();

                User user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    Object cardTypeAsObj;
                    bool isCardTypeValid = Enum.TryParse(typeof(CardType), cardDto.Type, out cardTypeAsObj);

                    if (!isCardTypeValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    CardType cardType = (CardType)cardTypeAsObj;

                    userCards.Add(new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = cardType
                    });
                }

                if (userCards.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User u = new User()
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = userCards
                };

                validUsers.Add(u);
                sb.AppendLine(String.Format(SuccessfullyImportedUser, u.Username, u.Cards.Count));
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();
            ImportPurchaseDto[] purchasesDtos = xmlHelper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");

            var validPurchases = new HashSet<Purchase>();

            foreach (var purchaseDto in purchasesDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Object purchaseTypeObj;
                bool isPurchaseValid = Enum.TryParse(typeof(PurchaseType), purchaseDto.Type, out purchaseTypeObj);

                if (!isPurchaseValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                PurchaseType purchaseType = (PurchaseType)purchaseTypeObj;

                DateTime purchaseDate;
                bool isDateValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Card card = context
                    .Cards
                    .FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = context
                    .Games
                    .FirstOrDefault(g => g.Name == purchaseDto.Title);

                if (game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Game = game,
                    Type = purchaseType,
                    ProductKey = purchaseDto.Key,
                    Card = card,
                    Date = purchaseDate
                };

                validPurchases.Add(purchase);
                sb.AppendLine(String.Format(SuccessfullyImportedPurchase, purchase.Game.Name, purchase.Card.User.Username));
            }

            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}