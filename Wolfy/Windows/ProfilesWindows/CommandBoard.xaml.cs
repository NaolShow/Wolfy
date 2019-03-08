using System;
using System.IO;
using System.Windows.Controls;

namespace Wolfy.Windows.ProfilesWindows {

    public partial class CommandBoard : UserControl {

        public CommandBoard(String _CommandPath) {
            InitializeComponent();

            CommandName.Content = Path.GetFileNameWithoutExtension(_CommandPath);
        }

    }
}
