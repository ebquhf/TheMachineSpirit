using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SpaceBattle.Common;
using System.Timers;
namespace TheMachineSpirit_Client
{
    public class TheMachineSpirit:IBattleClient
    {
        Random r = new Random();
        
        public string ClientName
        {
            get { return "The Machine Spirit "; }
        }

        public Brush ClientBrush
        {
            
            get { SolidBrush mySolidBrush = new SolidBrush(Color.Crimson); return mySolidBrush; }
        }
        int mapSize_X, mapSize_Y;
        int remainingTime;
        string message;
        List<BattleCommand> myBattleList = new List<BattleCommand>();
        List<GameItemDescriptor>myGameItems= new List<GameItemDescriptor>();
        List<GameItemDescriptor> myPlanets = new List<GameItemDescriptor>();
        List<GameItemDescriptor> myShips = new List<GameItemDescriptor>();
        Timer myTimer = new Timer(50);
        public List<BattleCommand> GetCommandsFromClient()
        {
            List<BattleCommand> temp = new List<BattleCommand>(myBattleList);
            myBattleList.Clear();
            return temp;
            
        }

        public void GiveGameItemsToClient(List<GameItemDescriptor> gameItems)
        {
            myGameItems = gameItems;
        }

        public void GiveMapSizeToClient(int size_x, int size_y)
        {
            mapSize_X = size_x; mapSize_Y = size_y;
            myTimer.Elapsed += new ElapsedEventHandler(OnElapsed);
            myTimer.Start();
            
        }

        public void GiveRemainingTimeToClient(int seconds)
        {
            remainingTime = seconds;
            
            
           
        }

        public void GiveMessageToClient(string msg)
        {
            message = msg;
        }
       /* public void Commander()
        {
            CmdMove temp = new CmdMove();
            temp.ItemId = myGameItems[0].ItemId;
            myBattleList.Add(temp);
        }*/
        private void OnElapsed(object sender, ElapsedEventArgs e) 
        {
            myTimer.Stop();
            int shipID=0;
            for (int i = 0; i < myGameItems.Count; i++)
            {


                CmdSplit temp = new CmdSplit();
                temp.ItemId = myGameItems[i].ItemId;
                temp.NumberOfUnits = 15;
                myBattleList.Add(temp);
               
            }
            
                
                for (int i = 0; i < myGameItems.Count ; i++)
                {

                    if (myGameItems[i].ItemType == "Ship")
                    {
                        CmdMove mover = new CmdMove();
                        mover.ItemId = myGameItems[i].ItemId;
                        mover.TargetX = myGameItems[i].PosX + r.Next(-4, 5);
                        mover.TargetY = myGameItems[i].PosY + r.Next(-3, 5);
                        myBattleList.Add(mover);
                        shipID = myGameItems[i].ItemId;
                        myShips.Add(myGameItems[i]);

                    }
                    else
                        myPlanets.Add(myGameItems[i]);
                    
                 
                }
                
            

           
                for (int i = 0; i < myGameItems.Count; i++)
                {



                    for (int j = 0; j < myShips.Count ; j++)
                    {



                        if (myGameItems[i].PlayerName != "The Machine Spirit ")
                        {
                            CmdMove moveToShoot = new CmdMove();
                            CmdShoot shooter = new CmdShoot();
                            shooter.ItemId = myShips[j].ItemId;
                            moveToShoot.ItemId = myShips[j].ItemId;
                            moveToShoot.TargetX = myGameItems[i].PosX;
                            moveToShoot.TargetY = myGameItems[i].PosY;
                            shooter.OtherItemId = myGameItems[i].ItemId;
                            shooter.NumberOfUnits = 15;
                            myBattleList.Add(moveToShoot);
                            myBattleList.Add(shooter);
                            //temp.ItemId = myGameItems[i].ItemId;
                        }

                    }
                }
                for (int i = 0; i <  myPlanets.Count ; i++)
                {
                   

                        if (myPlanets[i].PlayerName != "The Machine Spirit ")
                        {
                            CmdShoot shot = new CmdShoot();
                            CmdMove mvTSh = new CmdMove();
                            shot.ItemId = shipID;
                            mvTSh.ItemId = shipID;
                            mvTSh.TargetX = myPlanets[i].PosX;
                            mvTSh.TargetY = myPlanets[i].PosY;
                            
                            shot.NumberOfUnits = 15;
                            shot.OtherItemId = myPlanets[i].ItemId;
                            myBattleList.Add(mvTSh);
                            myBattleList.Add(shot);
                        }
                    
                }
               

                myTimer.Start();     
        }
    }
}
