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
        EditOwnPost = 0x020, // Includes the thread. (A thread has a first post)
        DeleteOwnPost = 0x040, // Includes the thread. (A thread has a first post)
        StickyThread = 0x080,
        MoveThread = 0x100,
        EditPost = 0x200, // Includes the thread. (A thread has a first post)
        DeletePost = 0x400, // Includes the thread. (A thread has a first post)
        StandardPermissions = ViewForum | SeeThread | ReadThread | CreateThread | ReplyThread | EditOwnPost | DeleteOwnPost,
        AllPermissions = StandardPermissions | StickyThread | MoveThread | EditPost | DeletePost
    }
}
