using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class Terminal
    {
        private int _terminalID;
        private bool _service = false; //Online 1 or Offline 0
        private bool _direction; //Entry 1 or Exit 0
        private Gate _gate; //RelationShip

        public Terminal(int terminalID, bool direction)
        {
            _terminalID = terminalID;
            _direction = direction;
            _gate = new Gate();
        }

        public bool Service { get { return this._service; } set { this._service = value; } }
        public bool Direction { get { return this._direction; } set { this._direction = value; } }
        public int TerminalID { get { return this._terminalID; } }

        public bool IsThereAVehicle()
        {
            return true;
        }

        //public string GetFaceID() //Invoke Python Script
        //{

        //    Process.Start("cmd.exe", "/C python demo.py");
        //    return "Ahmed";
        //}
        #region FaceRecognition
        public int LoadModel()
        {
            Process.Start("cmd.exe", "/C face_recognition.py model_flag");

            return 1;
        }
        public int Recognize()
        {
            //parameters needed for face recogmition
   

            Process.Start("cmd.exe", "/C C:\\Users\\ahmed\\Documents\\Visual Studio 2022\\Projects\\Parking System\\Aez\\face_recognition.py model_flag");
            //Invoke the face_recognition code to run and return the User ID if recognized or else return -1
            //if -1 is  returned Invoke Detection to create a new classfier with this new user
            String txt_file = System.IO.File.ReadAllText("ID.txt");
            if (txt_file == "-1")
            {
                return -1;
            }

            return int.Parse(txt_file);

        }

        public void AbortRecognition()
        {
            System.IO.File.WriteAllText("Recognition_stop.txt", "0");
        }
        #endregion
        #region FaceDetection

        public int FacesPreprocess(int ID) 
        {
            if (IsThereAVehicle())
            {
                Process.Start("cmd.exe", "/C data_preprocess.py ID");
                return 1;
            }
            return -1;
        }

        public int trainModel()
        {
            Process.Start("cmd.exe", "/C train_main.py");
            return 1;

        }

        #endregion

        public string GetLP() //Invoke Python Script
        {
            Process.Start("cmd.exe", "/C C:\\Users\\ahmed\\Documents\\Visual Studio 2022\\Projects\\Parking System\\Aez\\licence_plate.py");
            return "LP";
        }



        public void ControlGate(bool instruction)
        {
            if (instruction) _gate.Open(); 
            else _gate.Close() ;
        }


    }
}
