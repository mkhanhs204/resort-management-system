using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Rooms
{
    public class RoomEditVM : RoomCreateVM
    {
        public int RoomId { get; set; }

        [Display(Name = "Room Status")]
        public RoomStatus Status { get; set; }
    }
}
