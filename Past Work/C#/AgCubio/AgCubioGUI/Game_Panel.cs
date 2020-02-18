// Trevor Chapman, Xiaoyun Ding
// PS7
// 11/05/2015
// version 1.0

using System.Windows.Forms;

namespace AgCubioGUI
{
    /// <summary>
    /// A panel with DoubleBuffered set to true
    /// </summary>
    public class Game_Panel : Panel
    {
        /// <summary>
        /// Default constructor with DoubleBuffered = true set
        /// </summary>
        public Game_Panel()
        {
            this.DoubleBuffered = true;
        }
    }
}
