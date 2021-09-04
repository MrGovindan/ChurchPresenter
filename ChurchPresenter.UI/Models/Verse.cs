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
        private static readonly List<string> searchableBooks = new List<string>
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
            return s switch
            {
                "1 Chronicles" => Book.CHRONICLES1,
                "1 Corinthians" => Book.CORINTHIANS1,
                "1 John" => Book.JOHN1,
                "1 Kings" => Book.KINGS1,
                "1 Peter" => Book.PETER1,
                "1 Samuel" => Book.SAMUEL1,
                "1 Thessalonians" => Book.THESSALONIANS1,
                "1 Timothy" => Book.TIMOTHY1,
                "2 Chronicles" => Book.CHRONICLES2,
                "2 Corinthians" => Book.CORINTHIANS2,
                "2 John" => Book.JOHN2,
                "2 Kings" => Book.KINGS2,
                "2 Peter" => Book.PETER2,
                "2 Samuel" => Book.SAMUEL2,
                "2 Thessalonians" => Book.THESSALONIANS2,
                "2 Timothy" => Book.TIMOTHY2,
                "3 John" => Book.JOHN3,
                "Acts" => Book.ACTS,
                "Amos" => Book.AMOS,
                "Colossians" => Book.COLOSSIANS,
                "Daniel" => Book.DANIEL,
                "Deuteronomy" => Book.DEUTERONOMY,
                "Ecclesiastes" => Book.ECCLESIASTES,
                "Ephesians" => Book.EPHESIANS,
                "Esther" => Book.ESTHER,
                "Exodus" => Book.EXODUS,
                "Ezekiel" => Book.EZEKIEL,
                "Ezra" => Book.EZRA,
                "Galatians" => Book.GALATIANS,
                "Genesis" => Book.GENESIS,
                "Habakkuk" => Book.HABAKKUK,
                "Haggai" => Book.HAGGAI,
                "Hebrews" => Book.HEBREWS,
                "Hosea" => Book.HOSEA,
                "Isaiah" => Book.ISAIAH,
                "James" => Book.JAMES,
                "Jeremiah" => Book.JEREMIAH,
                "Job" => Book.JOB,
                "Joel" => Book.JOEL,
                "John" => Book.JOHN,
                "Jonah" => Book.JONAH,
                "Joshua" => Book.JOSHUA,
                "Jude" => Book.JUDE,
                "Judges" => Book.JUDGES,
                "Lamentations" => Book.LAMENTATIONS,
                "Leviticus" => Book.LEVITICUS,
                "Luke" => Book.LUKE,
                "Malachi" => Book.MALACHI,
                "Mark" => Book.MARK,
                "Matthew" => Book.MATTHEW,
                "Micah" => Book.MICAH,
                "Nahum" => Book.NAHUM,
                "Nehemiah" => Book.NEHEMIAH,
                "Numbers" => Book.NUMBERS,
                "Obadiah" => Book.OBADIAH,
                "Philemon" => Book.PHILEMON,
                "Philippians" => Book.PHILIPPIANS,
                "Proverbs" => Book.PROVERBS,
                "Psalms" => Book.PSALMS,
                "Revelation" => Book.REVELATION,
                "Romans" => Book.ROMANS,
                "Ruth" => Book.RUTH,
                "Song of Solomon" => Book.SONG_OF_SOLOMON,
                "Titus" => Book.TITUS,
                "Zechariah" => Book.ZECHARIAH,
                "Zephaniah" => Book.ZEPHANIAH,
                _ => null,
            };
        }

        internal static string ToString(Book book)
        {
            return book switch
            {
                Book.CHRONICLES1 => "1 Chronicles",
                Book.CORINTHIANS1 => "1 Corinthians",
                Book.JOHN1 => "1 John",
                Book.KINGS1 => "1 Kings",
                Book.PETER1 => "1 Peter",
                Book.SAMUEL1 => "1 Samuel",
                Book.THESSALONIANS1 => "1 Thessalonians",
                Book.TIMOTHY1 => "1 Timothy",
                Book.CHRONICLES2 => "2 Chronicles",
                Book.CORINTHIANS2 => "2 Corinthians",
                Book.JOHN2 => "2 John",
                Book.KINGS2 => "2 Kings",
                Book.PETER2 => "2 Peter",
                Book.SAMUEL2 => "2 Samuel",
                Book.THESSALONIANS2 => "2 Thessalonians",
                Book.TIMOTHY2 => "2 Timothy",
                Book.JOHN3 => "3 John",
                Book.ACTS => "Acts",
                Book.AMOS => "Amos",
                Book.COLOSSIANS => "Colossians",
                Book.DANIEL => "Daniel",
                Book.DEUTERONOMY => "Deuteronomy",
                Book.ECCLESIASTES => "Ecclesiastes",
                Book.EPHESIANS => "Ephesians",
                Book.ESTHER => "Esther",
                Book.EXODUS => "Exodus",
                Book.EZEKIEL => "Ezekiel",
                Book.EZRA => "Ezra",
                Book.GALATIANS => "Galatians",
                Book.GENESIS => "Genesis",
                Book.HABAKKUK => "Habakkuk",
                Book.HAGGAI => "Haggai",
                Book.HEBREWS => "Hebrews",
                Book.HOSEA => "Hosea",
                Book.ISAIAH => "Isaiah",
                Book.JAMES => "James",
                Book.JEREMIAH => "Jeremiah",
                Book.JOB => "Job",
                Book.JOEL => "Joel",
                Book.JOHN => "John",
                Book.JONAH => "Jonah",
                Book.JOSHUA => "Joshua",
                Book.JUDE => "Jude",
                Book.JUDGES => "Judges",
                Book.LAMENTATIONS => "Lamentations",
                Book.LEVITICUS => "Leviticus",
                Book.LUKE => "Luke",
                Book.MALACHI => "Malachi",
                Book.MARK => "Mark",
                Book.MATTHEW => "Matthew",
                Book.MICAH => "Micah",
                Book.NAHUM => "Nahum",
                Book.NEHEMIAH => "Nehemiah",
                Book.NUMBERS => "Numbers",
                Book.OBADIAH => "Obadiah",
                Book.PHILEMON => "Philemon",
                Book.PHILIPPIANS => "Philippians",
                Book.PROVERBS => "Proverbs",
                Book.PSALMS => "Psalms",
                Book.REVELATION => "Revelation",
                Book.ROMANS => "Romans",
                Book.RUTH => "Ruth",
                Book.SONG_OF_SOLOMON => "Song of Solomon",
                Book.TITUS => "Titus",
                Book.ZECHARIAH => "Zechariah",
                Book.ZEPHANIAH => "Zephaniah",
                _ => null,
            };
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
