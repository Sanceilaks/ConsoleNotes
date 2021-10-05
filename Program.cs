using System;


namespace ConsoleNotes
{
    class Program
    {
        static void printHeader() {
            Console.Write("exit | add | remove | get | save | load | list\n");
        }

        static string readValue(string name) {
            Console.Write(name);
            return Console.ReadLine();
        }

        static void error(string msg) {
            Console.WriteLine(msg);
            Console.ReadKey();
        }

        async static System.Threading.Tasks.Task Main(string[] args)
        {
            var storage = new NotesStorage();

            while (true) {
                Console.Clear();
                printHeader();

                var input = Console.ReadLine().ToLower();

                if (input.StartsWith("exit")) break;
                if (input.StartsWith("save")) await storage.Save();
                if (input.StartsWith("load")) await storage.Load();
                
                if (input.StartsWith("add"))
                {
                    var name = readValue("Name: ");
                    if (storage.NoteExist(name)) {
                        error("Note already exists!");
                        continue;
                    }
                    var note = storage.CreateNote(name);
                    note.Text = readValue("Text: ");
                    note.Date = System.DateTime.Today.ToShortDateString();
                }

                if (input.StartsWith("remove"))
                {
                    var name = readValue("Name: ");
                    if (!storage.NoteExist(name)) {
                        error("Note not exists!");
                        continue;
                    }
                    storage.Notes.Remove(storage.GetNoteByName(name));
                }

                if (input.StartsWith("get"))
                {
                    var name = readValue("Name: ");
                    if (!storage.NoteExist(name)) {
                        error("Note not exists!");
                        continue;
                    }
                    var note = storage.GetNoteByName(name);
                    Console.Write($"Name: {note.Name}\tDate: {note.Date}\n{note.Text}");
                    Console.ReadKey();
                }

                if (input.StartsWith("list"))
                {
                    foreach (var i in storage.Notes)
                        Console.WriteLine(i.ToString());
                    Console.ReadKey();
                }
            }
            await storage.Save();
        }
    }
}
