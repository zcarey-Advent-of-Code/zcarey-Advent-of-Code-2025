using System;
using System.Collections.Generic;
using System.Text;

namespace Day11 {

	[Flags]
	enum SeatState {
		Floor = 0x00,
		Empty = 0x10,
		Occupied = 0x20
	}
}
