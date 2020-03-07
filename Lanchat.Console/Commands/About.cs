using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public static void About()
        {
            string[] about = new string[25];
            about[0] = "";
            about[1] = "------------------------------------------------------------------------";
            about[2] = "|               .,-:;//;:=,                                            |";
            about[3] = "|           . :H@@@MM@M#H/.,+%;,                                       |";
            about[4] = "|        ,/X+ +M@@M@MM%=,-%HMMM@X/,                                    |";
            about[5] = "|      -+@MM; $M@@MH+-,;XMMMM@MMMM@+-                                  |";
            about[6] = "|     ;@M@@M- XM@X;. -+XXXXXHHH@M@M#@/.                                |";
            about[7] = "|   ,%MM@@MH ,@%=             .---=-=:=,.                              |";
            about[8] = "|   =@#@@@MX.,                -%HX$$%%%:;                              |";
            about[9] = "|  =-./@M@M$                   .;@MMMM@MM:    _____ ___  _____ _   _   |";
            about[10] = "|  X@/ -$MM/                    . +MM@@@M$   |_   _/ _ \\|  ___| | | |  |";
            about[11] = "| ,@M@H: :@:                    . =X#@@@@-     | || | | | |_  | | | |  |";
            about[12] = "| ,@@@MMX, .                    /H- ;@M@M=     | || |_| |  _| | |_| |  |";
            about[13] = "| .H@@@@M@+,                    %MM+..%#$.     |_| \\___/|_|    \\___/   |";
            about[14] = "|  /MMMM@MMH/.                  XM@MH; =;                              |";
            about[15] = "|   /%+%$XHH@$=              , .H@@@@MX,           LABORATORIES        |";
            about[16] = "|    .=--------.           -%H.,@@@@@MX,                               |";
            about[17] = "|    .%MM@@@HHHXX$$$%+- .:$MMX =M@@MM%.                                |";
            about[18] = "|      =XMMM@MM@MM#H;,-+HMM@M+ /MMMX=                                  |";
            about[19] = "|        =%@M@M#@$-.=$@MM@@@M; %M%=                                    |";
            about[20] = "|          ,:+$+-,/H#MMMMMMM@= =,                                      |";
            about[21] = "|                =++%%%%+/:-.                                          |";
            about[22] = "------------------------------------------------------------------------";
            about[23] = "";
            about[24] = "Lanchat 2 episode 1 confirmed";

            Prompt.Out(string.Join("\n", about));
        }
    }
}