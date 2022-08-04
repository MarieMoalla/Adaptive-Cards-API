using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaptiveCards;
using AdaptiveCardsFakeAPI.Entities;

namespace AdaptiveCardsFakeAPI.Services
{
    public interface IAdaptiveCardsService
    {
        public string generateUsersCheckListCard(IList<User> users);
    }
    public class AdaptiveCardsServices : IAdaptiveCardsService
    {
        #region GenerateUsersCheckListCard
        public string generateUsersCheckListCard(IList<User> users)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0)) 
            {
                Actions = new List<AdaptiveAction>()
        {
            new AdaptiveSubmitAction()
            {
                Title = "Submit",
            }
        }
            };

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Select Users",
                Size = AdaptiveTextSize.ExtraLarge,
                Wrap=true,
                Spacing= AdaptiveSpacing.Large
            });

            AdaptiveTextBlock textblock = null;

            AdaptiveChoiceSetInput choiceset = null;
            AdaptiveColumnSet columnset = null;

            

            foreach (User u in users)
            {
                textblock  = new AdaptiveTextBlock() 
                {
                    Text = u.userName 
                };

                 choiceset = new AdaptiveChoiceSetInput()
                {

                    Id = u.userName,
                    Style = AdaptiveChoiceInputStyle.Compact,
                    IsMultiSelect = true,
                    Choices = new List<AdaptiveChoice>()
                    {
                        new AdaptiveChoice()
                        {
                            Title = "Select",
                            Value = u.userName
                        }
                    }
                };
                columnset = new AdaptiveColumnSet()
                {
                    Columns = new List<AdaptiveColumn>()
                        {

                            new AdaptiveColumn()
                            {
                                Type="Column",
                                Items = {
                                    textblock
                                } ,
                                Width = AdaptiveColumnWidth.Auto
                            },
                            new AdaptiveColumn()
                            {
                                Type="Column",
                                Items = {
                                    choiceset
                                } ,
                                Width = AdaptiveColumnWidth.Auto
                            }
                        }
                };
                card.Body.Add(columnset);
            }
           
            string json = card.ToJson();
            return json;
}
        #endregion
    }
}
