using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public enum Book
    {
        GENESIS = 1,
        EXODUS,
        LEVITICUS,
        NUMBERS,
        DEUTERONOMY,
        JOSHUA,
        JUDGES,
        RUTH,
        SAMUEL1,
        SAMUEL2,
        KINGS1,
        KINGS2,
        CHRONICLES1,
        CHRONICLES2,
        EZRA,
        NEHEMIAH,
        ESTHER,
        JOB,
        PSALMS,
        PROVERBS,
        ECCLESIASTES,
        SONG_OF_SOLOMON,
        ISAIAH,
        JEREMIAH,
        LAMENTATIONS,
        EZEKIEL,
        DANIEL,
        HOSEA,
        JOEL,
        AMOS,
        OBADIAH,
        JONAH,
        MICAH,
        NAHUM,
        HABAKKUK,
        ZEPHANIAH,
        HAGGAI,
        ZECHARIAH,
        MALACHI,
        MATTHEW,
        MARK,
        LUKE,
        JOHN,
        ACTS,
        ROMANS,
        CORINTHIANS1,
        CORINTHIANS2,
        GALATIANS,
        EPHESIANS,
        PHILIPPIANS,
        COLOSSIANS,
        THESSALONIANS1,
        THESSALONIANS2,
        TIMOTHY1,
        TIMOTHY2,
        TITUS,
        PHILEMON,
        HEBREWS,
        JAMES,
        PETER1,
        PETER2,
        JOHN1,
        JOHN2,
        JOHN3,
        JUDE,
        REVELATION
    }

    public class BookHelper
    {
        private static List<string> searchableBooks = new List<string>
        {
        "1 Chronicles", "1 Corinthians", "1 John", "1 Kings", "1 Peter",
        "1 Samuel", "1 Thessalonians", "1 Timothy", "2 Chronicles", "2 Corinthians",
        "2 John", "2 Kings", "2 Peter", "2 Samuel", "2 Thessalonians",
        "2 Timothy", "3 John", "Acts", "Amos", "Colossians", "Daniel", "Deuteronomy",
        "Ecclesiastes", "Ephesians", "Esther", "Exodus", "Ezekiel", "Ezra",
        "Galatians", "Genesis", "Habakkuk", "Haggai", "Hebrews", "Hosea", "Isaiah",
        "James", "Jeremiah", "Job", "Joel", "John", "Jonah", "Joshua", "Jude",
        "Judges", "Lamentations", "Leviticus", "Luke", "Malachi", "Mark", "Matthew",
        "Micah", "Nahum", "Nehemiah", "Numbers", "Obadiah", "Philemon", "Philippians",
        "Proverbs", "Psalms", "Revelation", "Romans", "Ruth", "Song of Solomon",
        "Titus", "Zechariah", "Zephaniah",
        };

        public static IEnumerable<string> FindMatchingBook(string search)
        {
            search = search.ToLower();
            return searchableBooks.Where(book => book.ToLower().StartsWith(search)).ToArray();
        }

        public static Book? FromString(string s)
        {
            switch(s)
            {
                case "1 Chronicles": return Book.CHRONICLES1;
                case "1 Corinthians": return Book.CORINTHIANS1;
                case "1 John": return Book.JOHN1;
                case "1 Kings": return Book.KINGS1;
                case "1 Peter": return Book.PETER1;
                case "1 Samuel": return Book.SAMUEL1;
                case "1 Thessalonians": return Book.THESSALONIANS1;
                case "1 Timothy": return Book.TIMOTHY1;
                case "2 Chronicles": return Book.CHRONICLES2;
                case "2 Corinthians": return Book.CORINTHIANS2;
                case "2 John": return Book.JOHN2;
                case "2 Kings": return Book.KINGS2;
                case "2 Peter": return Book.PETER2;
                case "2 Samuel": return Book.SAMUEL2;
                case "2 Thessalonians": return Book.THESSALONIANS2;
                case "2 Timothy": return Book.TIMOTHY2;
                case "3 John": return Book.JOHN3;
                case "Acts": return Book.ACTS;
                case "Amos": return Book.AMOS;
                case "Colossians": return Book.COLOSSIANS;
                case "Daniel": return Book.DANIEL;
                case "Deuteronomy": return Book.DEUTERONOMY;
                case "Ecclesiastes": return Book.ECCLESIASTES;
                case "Ephesians": return Book.EPHESIANS;
                case "Esther": return Book.ESTHER;
                case "Exodus": return Book.EXODUS;
                case "Ezekiel": return Book.EZEKIEL;
                case "Ezra": return Book.EZRA;
                case "Galatians": return Book.GALATIANS;
                case "Genesis": return Book.GENESIS;
                case "Habakkuk": return Book.HABAKKUK;
                case "Haggai": return Book.HAGGAI;
                case "Hebrews": return Book.HEBREWS;
                case "Hosea": return Book.HOSEA;
                case "Isaiah": return Book.ISAIAH;
                case "James": return Book.JAMES;
                case "Jeremiah": return Book.JEREMIAH;
                case "Job": return Book.JOB;
                case "Joel": return Book.JOEL;
                case "John": return Book.JOHN;
                case "Jonah": return Book.JONAH;
                case "Joshua": return Book.JOSHUA;
                case "Jude": return Book.JUDE;
                case "Judges": return Book.JUDGES;
                case "Lamentations": return Book.LAMENTATIONS;
                case "Leviticus": return Book.LEVITICUS;
                case "Luke": return Book.LUKE;
                case "Malachi": return Book.MALACHI;
                case "Mark": return Book.MARK;
                case "Matthew": return Book.MATTHEW;
                case "Micah": return Book.MICAH;
                case "Nahum": return Book.NAHUM;
                case "Nehemiah": return Book.NEHEMIAH;
                case "Numbers": return Book.NUMBERS;
                case "Obadiah": return Book.OBADIAH;
                case "Philemon": return Book.PHILEMON;
                case "Philippians": return Book.PHILIPPIANS;
                case "Proverbs": return Book.PROVERBS;
                case "Psalms": return Book.PSALMS;
                case "Revelation": return Book.REVELATION;
                case "Romans": return Book.ROMANS;
                case "Ruth": return Book.RUTH;
                case "Song of Solomon": return Book.SONG_OF_SOLOMON;
                case "Titus": return Book.TITUS;
                case "Zechariah": return Book.ZECHARIAH;
                case "Zephaniah": return Book.ZEPHANIAH;
                default: return null;
            }
        }

        internal static string ToString(Book book)
        {
            switch (book)
            {
                case Book.CHRONICLES1: return "1 Chronicles";
                case Book.CORINTHIANS1: return "1 Corinthians";
                case Book.JOHN1: return "1 John";
                case Book.KINGS1: return "1 Kings";
                case Book.PETER1: return "1 Peter";
                case Book.SAMUEL1: return "1 Samuel";
                case Book.THESSALONIANS1: return "1 Thessalonians";
                case Book.TIMOTHY1: return "1 Timothy";
                case Book.CHRONICLES2: return "2 Chronicles";
                case Book.CORINTHIANS2: return "2 Corinthians";
                case Book.JOHN2: return "2 John";
                case Book.KINGS2: return "2 Kings";
                case Book.PETER2: return "2 Peter";
                case Book.SAMUEL2: return "2 Samuel";
                case Book.THESSALONIANS2: return "2 Thessalonians";
                case Book.TIMOTHY2: return "2 Timothy";
                case Book.JOHN3: return "3 John";
                case Book.ACTS: return "Acts";
                case Book.AMOS: return "Amos";
                case Book.COLOSSIANS: return "Colossians";
                case Book.DANIEL: return "Daniel";
                case Book.DEUTERONOMY: return "Deuteronomy";
                case Book.ECCLESIASTES: return "Ecclesiastes";
                case Book.EPHESIANS: return "Ephesians";
                case Book.ESTHER: return "Esther";
                case Book.EXODUS: return "Exodus";
                case Book.EZEKIEL: return "Ezekiel";
                case Book.EZRA: return "Ezra";
                case Book.GALATIANS: return "Galatians";
                case Book.GENESIS: return "Genesis";
                case Book.HABAKKUK: return "Habakkuk";
                case Book.HAGGAI: return "Haggai";
                case Book.HEBREWS: return "Hebrews";
                case Book.HOSEA: return "Hosea";
                case Book.ISAIAH: return "Isaiah";
                case Book.JAMES: return "James";
                case Book.JEREMIAH: return "Jeremiah";
                case Book.JOB: return "Job";
                case Book.JOEL: return "Joel";
                case Book.JOHN: return "John";
                case Book.JONAH: return "Jonah";
                case Book.JOSHUA: return "Joshua";
                case Book.JUDE: return "Jude";
                case Book.JUDGES: return "Judges";
                case Book.LAMENTATIONS: return "Lamentations";
                case Book.LEVITICUS: return "Leviticus";
                case Book.LUKE: return "Luke";
                case Book.MALACHI: return "Malachi";
                case Book.MARK: return "Mark";
                case Book.MATTHEW: return "Matthew";
                case Book.MICAH: return "Micah";
                case Book.NAHUM: return "Nahum";
                case Book.NEHEMIAH: return "Nehemiah";
                case Book.NUMBERS: return "Numbers";
                case Book.OBADIAH: return "Obadiah";
                case Book.PHILEMON: return "Philemon";
                case Book.PHILIPPIANS: return "Philippians";
                case Book.PROVERBS: return "Proverbs";
                case Book.PSALMS: return "Psalms";
                case Book.REVELATION: return "Revelation";
                case Book.ROMANS: return "Romans";
                case Book.RUTH: return "Ruth";
                case Book.SONG_OF_SOLOMON: return "Song of Solomon";
                case Book.TITUS: return "Titus";
                case Book.ZECHARIAH: return "Zechariah";
                case Book.ZEPHANIAH: return "Zephaniah";
                default: return null;
            }
        }
    }

    public readonly struct Verse
    {
        public readonly Book book;
        public readonly int chapter;
        public readonly int verse;

        public Verse(Book book, int chapter, int verse)
        {
            this.book = book;
            this.chapter = chapter;
            this.verse = verse;
        }

        public Verse(Verse other)
        {
            this.book = other.book;
            this.chapter = other.chapter;
            this.verse = other.verse;
        }
    }
}
