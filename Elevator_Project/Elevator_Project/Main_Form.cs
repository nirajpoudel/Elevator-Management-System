
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elevator_Project
{
    public partial class Main_Form : Form
    {
        public static Main_Form Self;

        public Main_Form()
        {
            InitializeComponent();
            Self = this;

            // On Starting of the program Set elevator acording to the database
            SetElevator();
            // set the label with the current floor number
            control_label.Text = "Elevator Current Floor = " + db.GetCurrentFloor();

            //  desable inside elevator control panel on start, enable after elevator is moved other floor
            button_floor_1.Enabled = false;
            button_floor_2.Enabled = false;
        }


        // *****************Class objects*********************
        Floors floor1 = new Floors(1, 337);
        Floors floor2 = new Floors(2, 0);
        Elevator elevator = new Elevator();
        Elevator_Database db = new Elevator_Database();

        // ****************Form Buttons **********************
        // button floor 1
        private void elevator_floor1_btn_up_Click(object sender, EventArgs e)
        {
            floor1.Open_Close_Gates();
            elevator.MoveToFloor(floor2);
        }

        // button floor 2
        private void elevator_floor2_btn_down_Click(object sender, EventArgs e)
        {
            floor2.Open_Close_Gates();
            elevator.MoveToFloor(floor1);
        }

        // button for displaying database log in to the dataview grid
        private void Display_db_Log_btn_Click(object sender, EventArgs e)
        {
            db.GetTablesData();
            Display_db_Log_btn.Text = "Reload Elevator Logs";
        }

        // button 1 of the elevator control panel
        private void button_floor_1_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(db.GetCurrentFloor()) == 2)
            {
                move_elevatorBox_from_floor2_to_floor1.Start();
            }
            else
            {
                control_label.Text = "Lift is already on Floor 1";
            }
        }
        // button 2 of the elevator control panel
        private void button_floor_2_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(db.GetCurrentFloor()) == 1)
            {
                move_elevatorBox_from_floor1_to_floor2.Start();
            }
            else
            {
                control_label.Text = "Lift is already on Floor 2";
            }
        }

        // ************Variables*****************
        bool IsElevatorDoorClosed = true;


        // ************Timers*****************

        // Timer for Open and Close Gate of the Floor 1
        private void floor1_door_open_close_timer_Tick(object sender, EventArgs e)
        {
            Buttons_On_Off("off");
            if (picturebox_left_floor1_door.Width >= 0 && IsElevatorDoorClosed == true && Int32.Parse(db.GetCurrentFloor()) == 1)
            {
                // Open Door
                picturebox_left_floor1_door.Width -= +3;
                picturebox_right_floor1_door.Width -= +3;
                picturebox_right_floor1_door.Left -= -3;
                if (picturebox_left_floor1_door.Width <= 0) {
                    IsElevatorDoorClosed = false;
                }
            }
            else if (picturebox_left_floor1_door.Width <= 141 && IsElevatorDoorClosed == false)
            {
                // Close door
                picturebox_left_floor1_door.Width += +3;
                picturebox_right_floor1_door.Width += +3;
                picturebox_right_floor1_door.Left += -3;
                if (picturebox_left_floor1_door.Width >= 142)
                {
                    // Stop This timer
                    IsElevatorDoorClosed = true;
                    floor1_door_open_close_timer.Stop();
                    Buttons_On_Off("on");
                    //  enable elevator inside control panel buttons
                    button_floor_1.Enabled = true;
                    button_floor_2.Enabled = true;
                }
            }
        }

        // Timer for Open and Close Gate of the Floor 2
        private void floor2_door_open_close_timer_Tick(object sender, EventArgs e)
        {
            Buttons_On_Off("off");
            if (picturebox_left_floor2_door.Width >= 0 && IsElevatorDoorClosed == true && Int32.Parse(db.GetCurrentFloor()) == 2)
            {
                // Open Door
                picturebox_left_floor2_door.Width -= +3;
                picturebox_right_floor2_door.Width -= +3;
                picturebox_right_floor2_door.Left -= -3;
                if (picturebox_left_floor2_door.Width <= 0)
                {
                    IsElevatorDoorClosed = false;
                }
            }
            else if (picturebox_left_floor2_door.Width <= 141 && IsElevatorDoorClosed == false)
            {
                // Close door
                picturebox_left_floor2_door.Width += +3;
                picturebox_right_floor2_door.Width += +3;
                picturebox_right_floor2_door.Left += -3;
                if (picturebox_left_floor2_door.Width >= 142)
                {
                    // Stop This timer
                    IsElevatorDoorClosed = true;
                    floor2_door_open_close_timer.Stop();
                    Buttons_On_Off("on");
                    //  enable elevator inside control panel buttons
                    button_floor_1.Enabled = true;
                    button_floor_2.Enabled = true;
                }
            }
        }

        // Timer for moving the elevator from floor 2 to floor 1
        private void move_elevatorBox_from_floor2_to_floor1_Tick(object sender, EventArgs e)
        {
            Buttons_On_Off("off");
            control_label.Text = "Going to Floor 1";
            if (floor2_door_open_close_timer.Enabled == false)
            {
                elevator_box_picturebox.Top += 2;
                if (elevator_box_picturebox.Location.Y >= floor1.GetFloorYLocation()) {
                    move_elevatorBox_from_floor2_to_floor1.Stop();
                    floor1.Open_Close_Gates();
                    db.SetCurrentFloor(1);
                    db.GetTablesData();
                    Buttons_On_Off("on");
                    Btn_ColorLabel_reset();
                    control_label.Text = "Elevator Current Floor = " + db.GetCurrentFloor();

                }
            }
        }

        // Timer for moving the elevator from floor 1 to floor 2
        private void move_elevatorBox_from_floor1_to_floor2_Tick(object sender, EventArgs e)
        {
            Buttons_On_Off("off");
            control_label.Text = "Going to Floor 2";
            if (floor1_door_open_close_timer.Enabled == false)
            {
                elevator_box_picturebox.Top -= 2;
                if (elevator_box_picturebox.Location.Y <= floor2.GetFloorYLocation()){
                    move_elevatorBox_from_floor1_to_floor2.Stop();
                    floor2.Open_Close_Gates();
                    db.SetCurrentFloor(2);
                    db.GetTablesData();
                    Buttons_On_Off("on");
                    Btn_ColorLabel_reset();
                    control_label.Text = "Elevator Current Floor = "+db.GetCurrentFloor();
                }
            }
        }

        // **************Functions***************

        // fuction to set the elevatorBox on program run.
        private void SetElevator() {
            try
            {
                if (Int32.Parse(db.GetCurrentFloor()) == 1)
                {
                    elevator_box_picturebox.Location = new System.Drawing.Point(0, floor1.GetFloorYLocation());
                }
                else if (Int32.Parse(db.GetCurrentFloor()) == 2)
                {
                    elevator_box_picturebox.Location = new System.Drawing.Point(0, floor2.GetFloorYLocation());
                }
            }
            catch {
                // by default add the elevator to floor 0
                elevator_box_picturebox.Location = new System.Drawing.Point(0, floor1.GetFloorYLocation());
                // and add to the db for other functions to work correctly
                db.SetCurrentFloor(1);
            }
        }

        // function which will desable or enable buttons of the form while elevator is moving or doors are closing or opening
        private void Buttons_On_Off(string btn) {
            if (btn == "on") {
                elevator_floor1_btn_up.Click += elevator_floor1_btn_up_Click;
                elevator_floor2_btn_down.Click += elevator_floor2_btn_down_Click;
            }
            else if (btn == "off")
            {
                elevator_floor1_btn_up.Click -= elevator_floor1_btn_up_Click;
                elevator_floor2_btn_down.Click -= elevator_floor2_btn_down_Click;
            }
        }

        // function to change the elevator weiting bg color to default and reset the labels
        private void Btn_ColorLabel_reset() {
            if (elevator_floor2_btn_down.BackColor == Color.Lime || elevator_floor1_btn_up.BackColor == Color.Lime) {
                elevator_floor2_btn_down.BackColor = Color.Transparent;
                elevator_floor1_btn_up.BackColor = Color.Transparent;
            }
        }
        
    }
}
