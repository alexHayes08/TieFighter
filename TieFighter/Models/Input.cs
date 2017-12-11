using System.Runtime.InteropServices;
using TieFighter.Models.Settings;
using KeyCode = System.Int32;

namespace TieFighter.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        [FieldOffset(0)]
        private KeyCode? _KeyCode;

        [FieldOffset(0)]
        private MouseButtons? _MouseButton;

        public KeyCode? KeyCode
        {
            get
            {
                return _KeyCode;
            }
            set
            {
                _MouseButton = null;
                _KeyCode = value;
            }
        }

        public MouseButtons? MouseButton
        {
            get
            {
                return _MouseButton;
            }
            set
            {
                _KeyCode = null;
                _MouseButton = value;
            }
        }
    }
}
