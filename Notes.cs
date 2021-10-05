using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleNotes
{
    public class Note 
    {
        public string Name {get; set;}
        public string Text {get; set;}
        public string Date {get; set;}
        public override string ToString() => $"{Name} {Date} {Text}";
    }
    
    public class NotesStorage {
        public List<Note> Notes {get; private set;}

        [NonSerialized]
        static string fileName = "data.vd";
        public NotesStorage() {
            Notes = new List<Note>();
            if (File.Exists(fileName))
                Task.Run(() => this.Load()).Wait();
        }

        async public Task Save() {
            using var stream = File.OpenWrite(fileName);
            await JsonSerializer.SerializeAsync<List<Note>>(stream, Notes);
        }

        async public Task Load() {
            using var stream = File.OpenRead(fileName);
            Notes.AddRange(from i in await JsonSerializer.DeserializeAsync<List<Note>>(stream)
                           where !NoteExist(i.Name)
                           select i);
        }

        public bool NoteExist(string name) =>
            Notes.Exists(delegate (Note n) {
                return n.Name.ToLower().Equals(name.ToLower());
            });
        
        public Note CreateNote(string name)
        {
            Notes.Add(new Note() { Name = name });
            return Notes[Notes.Count - 1];
        }

        public Note GetNoteByName(string name) =>
            Notes.FindLast(delegate (Note n) {
                return n.Name.ToLower().Equals(name.ToLower());
            });
    }
}