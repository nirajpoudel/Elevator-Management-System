using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project
{
    class Elevator
    {
        // get object of the db class to get the current floor number
        Elevator_Database db = new Elevator_Database();
        
        // method to run the timer of the elevator acording to the floor number of the floor object
        internal void MoveToFloor(Floors floor) {
            if (floor.GetFloorNumber() == 1 && Int32.Parse(db.GetCurrentFloor()) == 2)
            {
                Main_Form.Self.move_elevatorBox_from_floor2_to_floor1.Start();
            }

            else if (floor.GetFloorNumber() == 2 && Int32.Parse(db.GetCurrentFloor()) == 1) {
                Main_Form.Self.move_elevatorBox_from_floor1_to_floor2.Start();
            }

            else if (floor.GetFloorNumber() == 1 && Int32.Parse(db.GetCurrentFloor()) == 1)
            {
                Main_Form.Self.elevator_floor2_btn_down.BackColor = System.Drawing.Color.Lime;
                Main_Form.Self.move_elevatorBox_from_floor1_to_floor2.Start();
            }
            else if (floor.GetFloorNumber() == 2 && Int32.Parse(db.GetCurrentFloor()) == 2)
            {
                Main_Form.Self.elevator_floor1_btn_up.BackColor = System.Drawing.Color.Lime;
                Main_Form.Self.move_elevatorBox_from_floor2_to_floor1.Start();
            }

        }


    }
}
