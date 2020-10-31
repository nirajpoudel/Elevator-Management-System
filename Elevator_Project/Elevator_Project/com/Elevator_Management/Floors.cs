using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project
{
    class Floors
    {
        
        int FloorNumber;
        int FloorStartLocationY;

        // constuctor for setting floor number and location of the floor
        public Floors(int nmbr, int location) {
            FloorNumber = nmbr;
            FloorStartLocationY = location;
        }

        // get the location where the elevator start
        internal int GetFloorYLocation() {
            return FloorStartLocationY;
        }
        // get floor number of this floor
        internal int GetFloorNumber()
        {
            return FloorNumber;
        }
        // open and close button for the specific floor
        internal void Open_Close_Gates() {
            if (FloorNumber == 1) {
                // timer of floor 1 for open and close gate.
                Main_Form.Self.floor1_door_open_close_timer.Start();
            }
            else if (FloorNumber == 2)
            {
                // start floor 2 close open gate timer.
                Main_Form.Self.floor2_door_open_close_timer.Start();

            }
        }
    }
}
