using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wolfy.Classes;
using Wolfy.Files.Json;

namespace Wolfy.Windows.ProfilesWindows {

    public partial class CommandBoard : UserControl {

        // |-------[ Variables ]-------| //
        private JsonCommand Command;

        public CommandBoard(JsonCommand _Command, ListBoxItem _Item) {
            InitializeComponent();

            // Save command
            Command = _Command;

            #region Events

            // Command name
            CommandNameBox.Text = Command.CommandName;
            ApplyCommandBtn.Click += delegate {

                // Command name
                string _NewCommandPath = Path.Combine(Profiles.GetProfilePath(), CommandNameBox.Text);
                if (Command.CommandPath != _NewCommandPath) {

                    if (!Directory.Exists(_NewCommandPath)) {

                        // Change command name
                        _Item.Content = CommandNameBox.Text;
                        Command.CommandName = CommandNameBox.Text;

                        // Change directory
                        Directory.Move(Command.CommandPath, _NewCommandPath);
                        Command.CommandPath = _NewCommandPath;

                    } else {
                        Utils.ErrorLabel(CommandErrorLabel, Langs.Get("command_already_exist"), 5);
                    }

                }

            };

            // Add command variable
            CommandAddBtn.Click += delegate {

                // Key is not null & Key doesn't exist
                string _Key = KeyVariableBox.Text.Replace(" ", "");
                if (!string.IsNullOrEmpty(_Key) && !Command.Command_vars.Keys.Contains(_Key)) {

                    KeyValuePair<string, string> _Pair = new KeyValuePair<string, string>(_Key, ValueVariableBox.Text);
                    Command.Command_vars.Add(_Pair.Key, _Pair.Value);
                    AddVariable(_Pair);

                } else {
                    Utils.ErrorLabel(CommandAddError, Langs.Get("variable_already_exist"), 5);
                }

            };

            #endregion

            #region Lua variables

            // Add variables
            foreach (KeyValuePair<string, string> _Pair in Command.Command_vars) {
                AddVariable(_Pair);
            }

            #endregion

        }

        #region Variables

        private void AddVariable(KeyValuePair<string, string> _Pair) {

            // _Panel
            WrapPanel _Panel = new WrapPanel();

            #region Box

            // Key Box UI
            TextBox _KeyBox = new TextBox() {
                Text = _Pair.Key,
                IsEnabled = false,
                Width = 128
            };

            // Value Box UI
            TextBox _ValueBox = new TextBox() {
                Text = _Pair.Value,
                Width = 156
            };
            // Value Box Code
            _ValueBox.TextChanged += delegate {
                Command.Command_vars[_Pair.Key] = _ValueBox.Text;
            };

            #endregion

            #region Remove

            // Button UI
            Button _RemoveBtn = new Button() {
                Content = new PackIcon { Kind = PackIconKind.CloseOutline },
                Style = (Style)FindResource("MaterialDesignFlatButton"),
                Margin = new Thickness(6, 0, 0, 0)
            };

            // Button Code
            _RemoveBtn.Click += delegate {

                // Remove in code
                Command.Command_vars.Remove(_Pair.Key);

                // Remove in UI
                VarsPanel.Children.Remove(_Panel);

            };

            #endregion
            #region Up

            // Button UI
            Button _UpBtn = new Button() {
                Content = new PackIcon { Kind = PackIconKind.ArrowUpBoldOutline },
                Style = (Style)FindResource("MaterialDesignFlatButton")
            };
            // Button Code
            _UpBtn.Click += delegate {

                int _VarIndex = Command.Command_vars.Keys.ToList().IndexOf(_Pair.Key);

                // Command on top
                if (Command.Command_vars.ElementAtOrDefault(_VarIndex - 1).Key != null) {

                    // Swap in code
                    Command.Command_vars = SwapVariable(Command.Command_vars, _VarIndex, _VarIndex - 1);

                    // Swap in UI
                    int _Index = VarsPanel.Children.IndexOf(_Panel);
                    VarsPanel.Children.RemoveAt(_Index);
                    VarsPanel.Children.Insert(_Index - 1, _Panel);

                }

            };

            #endregion
            #region Down

            // Button UI
            Button _DownBtn = new Button() {
                Content = new PackIcon { Kind = PackIconKind.ArrowDownBoldOutline },
                Style = (Style)FindResource("MaterialDesignFlatButton")
            };
            // Button Code
            _DownBtn.Click += delegate {

                int _VarIndex = Command.Command_vars.Keys.ToList().IndexOf(_Pair.Key);

                // Command on down
                if (Command.Command_vars.ElementAtOrDefault(_VarIndex + 1).Key != null) {

                    // Swap & save
                    Command.Command_vars = SwapVariable(Command.Command_vars, _VarIndex, _VarIndex + 1);

                    // Swap in UI
                    int _Index = VarsPanel.Children.IndexOf(_Panel);
                    VarsPanel.Children.RemoveAt(_Index);
                    VarsPanel.Children.Insert(_Index + 1, _Panel);

                }

            };

            #endregion

            // Add to _Panel
            _Panel.Children.Add(_KeyBox);
            _Panel.Children.Add(new Label() { Content = "  :  ", VerticalContentAlignment = VerticalAlignment.Bottom });
            _Panel.Children.Add(_ValueBox);
            _Panel.Children.Add(_RemoveBtn);
            _Panel.Children.Add(_UpBtn);
            _Panel.Children.Add(_DownBtn);

            // Add to Window
            VarsPanel.Children.Add(_Panel);

        }

        private Dictionary<string, string> SwapVariable(Dictionary<string, string> _Dict, int _IndexA, int _IndexB) {

            // Get list
            var _List = _Dict.ToList();

            // Swap
            var _EntryBack = _List[_IndexA];
            _List[_IndexA] = _List[_IndexB];
            _List[_IndexB] = _EntryBack;

            // Convert & return
            return _List.ToDictionary(a => a.Key, a => a.Value);

        }

        #endregion

        public void SaveCommand() {

            // Command still exist (Prevent crashing)
            if (Directory.Exists(Command.CommandPath)) {

                // Save files
                File.WriteAllText(Path.Combine(Command.CommandPath, "command.info"), JsonConvert.SerializeObject(Command, Formatting.Indented));

            }

        }

    }
}
