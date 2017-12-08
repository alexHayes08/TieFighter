using System.Runtime.InteropServices;
using TieFighter.Models.Settings;
using KeyCode = System.Int32;

namespace TieFighter.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        [FieldOffset(0)]
        public KeyCode KeyCode;

        [FieldOffset(0)]
        public MouseButtons MouseButton;
    }
}
