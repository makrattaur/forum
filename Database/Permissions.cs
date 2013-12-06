using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    [Flags]
    public enum Permissions
    {
        ViewForum = 0x001,
        SeeThread = 0x002,
        ReadThread = 0x004,
        CreateThread = 0x008,
        ReplyThread = 0x010,
        EditOwnThread = 0x020,
        DeleteOwnThread = 0x040,
        StickyThread = 0x080,
        MoveThread = 0x100,
        EditThread = 0x200,
        DeleteThread = 0x400,
        StandardPermissions = ViewForum | SeeThread | ReadThread | CreateThread | ReplyThread | EditOwnThread | DeleteOwnThread,
        AllPermissions = StandardPermissions | StickyThread | MoveThread | EditThread | DeleteThread
    }
}
