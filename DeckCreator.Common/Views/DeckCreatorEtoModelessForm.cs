using Eto.Drawing;
using Eto.Forms;
using DeckCreator.Common.Methods;

namespace DeckCreator.Common.Views
{
    class DeckCreatorEtoModelessForm : Form
    {
        public DeckCreatorEtoModelessForm()
        {
            Maximizable = false;
            Minimizable = false;
            Padding = new Padding(5);
            Resizable = false;
            ShowInTaskbar = true;
            Title = "Deck creator";
            WindowStyle = WindowStyle.Default;

            #region Deck params
            var deck_Name = new TextBox
            {
                ToolTip = "Enter deck name.",
            };
            var deck_Height = new TextBox
            {
                ToolTip = "Enter deck height.",
            };
            #endregion

            #region Buttons
            var add_Button = new Button { Text = "Add deck" };
            add_Button.Click += (sender, e) => CreateDeck(deck_Name, deck_Height);

            var close_Button = new Button { Text = "Close" };
            close_Button.Click += (sender, e) => Close();
            #endregion

            #region Deck params group box
            var deck_GroupBox = new GroupBox
            {
                Text = "Deck parameters",
                Content = new TableLayout
                {
                    Rows =
                    {
                        new TableRow
                        {
                            Cells =
                            {
                                new Label{Text = "Name", TextAlignment = TextAlignment.Left},
                                new TableCell(deck_Name),
                            }
                        },
                        new TableRow
                        {
                            Cells =
                            {
                                new Label{Text = "Height", TextAlignment= TextAlignment.Left},
                                new TableCell(deck_Height),
                            }
                        }
                    }
                }
            };
            #endregion

            #region
            var bottom_Buttons = new TableLayout
            {
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(add_Button, true),
                            new TableCell(close_Button, true),
                        }
                    }
                }
            };
            #endregion

            var layout = new TableLayout
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(10),

                Rows =
                {
                    deck_GroupBox,
                    null,
                    bottom_Buttons,
                },
            };

            Content = layout;
        }

        void CreateDeck(TextBox name, TextBox height)
        {
            CreateDeckClass createDeck = new CreateDeckClass(name.Text, int.Parse(height.Text));
            createDeck.CreateDeck();
            return;
        }
    }
}
