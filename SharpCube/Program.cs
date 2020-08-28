using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

//This program was made by zix#6969
//If you want something made, DM me on discord, and I might consider doing it.
//Don't mind the messy code :P

namespace SharpCube
{
    class Program
    {
        public static int ounlammo = 0x637E9;
        public static int olocal = 0x10F4F4;
        public static int ohealth = 0xF8;
        public static int recoilas = 0x462020;

        static void Main(string[] args)
        {
            VAMemory vam = new VAMemory("ac_client");
            Process gameProcess;

            if (Process.GetProcessesByName("ac_client").FirstOrDefault() == null)
            {
                Console.WriteLine("Waiting for game...");
                while (Process.GetProcessesByName("ac_client").FirstOrDefault() == null)
                {
                    Thread.Sleep(300);
                }
            }

            Console.Clear();
            gameProcess = Process.GetProcessesByName("ac_client").FirstOrDefault();
            Thread.Sleep(100);
            IntPtr baseAddress = gameProcess.MainModule.BaseAddress;
            IntPtr unlammo = baseAddress + ounlammo;
            int health = vam.ReadInt32(baseAddress + olocal) + ohealth;

            bool hp = false;
            bool ammo = false;
            bool recoil = false;
            bool all = false;

        bacl:
            Console.WriteLine("Made by zix#6969\n", Console.ForegroundColor = ConsoleColor.Yellow);

            if (hp == true)
            {
                Console.WriteLine("1 - Unlimited Health (SinglePlayer only!)", Console.ForegroundColor = ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("1 - Unlimited Health (SinglePlayer only!)", Console.ForegroundColor = ConsoleColor.Red);
            }

            if (ammo == true)
            {
                Console.WriteLine("2 - Unlimited Ammo", Console.ForegroundColor = ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("2 - Unlimited Ammo", Console.ForegroundColor = ConsoleColor.Red);
            }

            if (recoil == true)
            {
                Console.WriteLine("3 - No Recoil", Console.ForegroundColor = ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("3 - No Recoil", Console.ForegroundColor = ConsoleColor.Red);
            }

            if (all == true)
            {
                Console.WriteLine("\n0 - All Hacks", Console.ForegroundColor = ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("\n0 - All Hacks", Console.ForegroundColor = ConsoleColor.Red);
            }

            Console.Write("\nSelect hack number: ", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.ResetColor();

            string dec = Console.ReadLine();

            Thread t = new Thread(hphack);

            switch (dec)
            {
                case "1":
                    if (hp != true)
                    {
                        hp = true;
                        t.Start();
                    }
                    else
                    {
                        hp = false;
                        t.Abort();
                        vam.WriteInt32((IntPtr)health, 100);
                    }
                    break;

                case "2":
                    if (ammo != true)
                    {
                        ammohack();
                        ammo = true;
                    }
                    else
                    {
                        ammohack();
                        ammo = false;
                    }
                    break;

                case "3":
                    if (recoil != true)
                    {
                        recoilhack();
                        recoil = true;
                    }
                    else
                    {
                        recoilhack();
                        recoil = false;
                    }
                    break;

                case "0":
                    if (all != true)
                    {
                        t.Start();
                        ammohack();
                        recoilhack();
                        all = true;
                        hp = true;
                        ammo = true;
                        recoil = true;
                    }
                    else
                    {
                        t.Abort();
                        vam.WriteInt32((IntPtr)health, 100);
                        ammohack();
                        recoilhack();
                        all = false;
                        hp = false;
                        ammo = false;
                        recoil = false;
                    }
                    break;
            }
            Console.Clear();
            goto bacl;

            void hphack()
            {
                while (true)
                {
                    vam.WriteInt32((IntPtr)health, 9999);
                }
            }

            void ammohack()
            {
                if (ammo != true)
                {
                    byte[] activated = { 0x90, 0x90 };
                    vam.WriteByteArray(unlammo, activated);
                }
                else
                {
                    byte[] disabled = { 0xFF, 0x0E };
                    vam.WriteByteArray(unlammo, disabled);
                }
            }

            void recoilhack()
            {
                if (recoil != true)
                {
                    byte[] bitai = { 0xC2, 0x08, 0x00 };
                    vam.WriteByteArray((IntPtr)recoilas, bitai);
                }
                else
                {
                    byte bitas = 0x55;
                    byte[] testas = { 0x8B, 0xEC };
                    vam.WriteByte((IntPtr)recoilas, bitas);
                    vam.WriteByteArray((IntPtr)0x462021, testas);
                }
            }
        }
    }
}