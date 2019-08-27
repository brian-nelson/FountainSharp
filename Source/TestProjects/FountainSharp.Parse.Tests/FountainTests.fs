module FountainSharp.Parse.Tests.Unsorted

open System
open FsUnit
open NUnit.Framework
open FountainSharp
open FountainSharp.Parse
open FountainSharp.Parse.Helper

//===== Block Elements

[<Test>]
let ``Empty lines`` () =
   let doc = NewLine(2) |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [ Action(false, [ HardLineBreak(new Range(0, NewLineLength)); HardLineBreak(new Range(NewLineLength, NewLineLength)) ], new Range(0, NewLineLength * 2)) ]

//===== Scene Headings

[<Test>]
let ``Forced (".") Scene Heading`` () =
   let doc = properNewLines "\r\n.BINOCULARS A FORCED SCENE HEADING - LATER\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (true, [Literal ("BINOCULARS A FORCED SCENE HEADING - LATER", new Range(1 + NewLineLength, 41))], new Range(0, 42 + NewLineLength * 2)) ]

[<Test>]
let ``Forced (".") Scene Heading with line breaks and action`` () =
   let text = properNewLines "\r\n.BRICK'S PATIO - DAY\r\n\r\nSome Action"
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [ SceneHeading (true, [Literal ("BRICK'S PATIO - DAY", new Range(1 + NewLineLength, 19))], new Range(0, 20 + NewLineLength * 3)); Action (false, [Literal ("Some Action", new Range(20 + NewLineLength * 3, 11))], new Range(20 + NewLineLength * 3, 11))]

[<Test>]
let ``Forced (".") Scene Heading with more line breaks and action`` () =
   let doc = properNewLines "\r\n.BRICK'S PATIO - DAY\r\n\r\n\r\nSome Action" |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [SceneHeading (true, [Literal ("BRICK'S PATIO - DAY", new Range(1 + NewLineLength, 19))], new Range(0, 20 + 3 * NewLineLength)); Action(false, [HardLineBreak(new Range(20 + NewLineLength * 3, NewLineLength)); Literal("Some Action", new Range(20 + NewLineLength * 4, 11))], new Range(20 + NewLineLength * 3, 11 + NewLineLength))]

[<Test>]
let ``Forced (".") Scene Heading - No empty line after`` () =
   let text = properNewLines ".BINOCULARS A FORCED SCENE HEADING - LATER\r\nSome Action"
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [Action (false, [Literal (".BINOCULARS A FORCED SCENE HEADING - LATER", new Range(0, 42)); HardLineBreak(new Range(42, NewLineLength)); Literal ("Some Action", new Range(42 + NewLineLength, 11))], new Range(0, text.Length))]

[<Test>]
let ``Lowercase known scene heading`` () =
   let doc = properNewLines "\r\next. brick's pool - day\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("ext. brick's pool - day", new Range(NewLineLength, 23))], new Range(0, 23 + NewLineLength * 2)) ]

[<Test>]
let ``Known INT Scene Head`` () =
   let doc = properNewLines "\r\nINT DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("INT DOGHOUSE - DAY", new Range(NewLineLength, 18))], new Range(0, 18 + NewLineLength * 2))]

[<Test>]
let ``Known EXT Scene Head`` () =
   let doc = properNewLines "\r\nEXT DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("EXT DOGHOUSE - DAY", new Range(NewLineLength, 18))], new Range(0, 18 + NewLineLength * 2)) ]

[<Test>]
let ``Known EST Scene Head`` () =
   let doc = properNewLines "\r\nEST DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("EST DOGHOUSE - DAY", new Range(NewLineLength, 18))], new Range(0, 18 + NewLineLength * 2)) ]

[<Test>]
let ``Known INT./EXT Scene Head`` () =
   let doc = properNewLines "\r\nINT./EXT DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [SceneHeading (false, [Literal ("INT./EXT DOGHOUSE - DAY", new Range(NewLineLength, 23))], new Range(0, 23 + NewLineLength * 2))]

[<Test>]
let ``Known INT/EXT Scene Head`` () =
   let doc = properNewLines "\r\nINT/EXT DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [SceneHeading (false, [Literal ("INT/EXT DOGHOUSE - DAY", new Range(NewLineLength, 22))], new Range(0, 22 + NewLineLength * 2))]

[<Test>]
let ``Known I/E Scene Head`` () =
   let doc = properNewLines "\r\nI/E DOGHOUSE - DAY\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("I/E DOGHOUSE - DAY", new Range(NewLineLength, 18))], new Range(0, 18 + NewLineLength * 2)) ]

[<Test>]
let ``Scene Heading with line breaks and action`` () =
   let doc = properNewLines "\r\nEXT. BRICK'S PATIO - DAY\r\n\r\nSome Action" |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [SceneHeading (false, [Literal ("EXT. BRICK'S PATIO - DAY", new Range(NewLineLength, 24))], new Range(0, 24 + NewLineLength * 3)); Action (false, [Literal ("Some Action", new Range(24 + NewLineLength * 3, 11))], new Range(24 + NewLineLength * 3, 11))]

[<Test>]
let ``Scene Heading with more line breaks and action`` () =
   let doc = properNewLines "\r\nEXT. BRICK'S PATIO - DAY\r\n\r\n\r\nSome Action" |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [SceneHeading (false, [Literal ("EXT. BRICK'S PATIO - DAY", new Range(NewLineLength, 24))], new Range(0, 24 + NewLineLength * 3)); Action(false, [HardLineBreak(new Range(24 + NewLineLength * 3, NewLineLength)); Literal("Some Action", new Range(24 + NewLineLength * 4, 11))], new Range(24 + NewLineLength * 3, NewLineLength + 11))]

[<Test>]
let ``Scene Heading - No empty line after`` () =
   // this must not be recognized as scene heading
   let text = properNewLines "EXT. BRICK'S PATIO - DAY\r\nSome Action"
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [Action (false, [Literal ("EXT. BRICK'S PATIO - DAY", new Range(0, 24)); HardLineBreak(new Range(24, NewLineLength)); Literal ("Some Action", new Range(24 + NewLineLength, 11))], new Range(0, text.Length))]

[<Test>]
let ``Scene Heading - Character after`` () =
   let doc = properNewLines "\r\nEXT. BRICK'S PATIO - DAY\r\n\r\nLINDSEY" |> FountainDocument.Parse
   doc.Blocks 
   |> should equal [ SceneHeading(false, [ Literal("EXT. BRICK'S PATIO - DAY", new Range(NewLineLength, 24)) ], new Range(0, 24 + NewLineLength * 3)); Character(false, true, [ Literal("LINDSEY", new Range(24 + NewLineLength * 3, 7)) ], new Range(24 + NewLineLength * 3, 7)) ]

//===== Synopses

[<Test>]
let ``Basic Synopses`` () =
   let doc = "= Here is a synopses of this fascinating scene." |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Synopses ([Literal ("Here is a synopses of this fascinating scene.", new Range(2, 45))], new Range(0, 47))]


//===== Parenthetical

[<Test>]
let ``Parenthetical `` () =
   let doc = properNewLines "\r\nLINDSEY\r\n(quietly)" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ Character (false, true, [Literal ("LINDSEY", new Range(NewLineLength, 7))], new Range(0, 7 + NewLineLength * 2)); Parenthetical ([Literal ("quietly", new Range(8 + NewLineLength * 2, 7))], new Range(7 + NewLineLength * 2, 9))];

[<Test>]
let ``Parenthetical - After Dialogue`` () =
   let doc = properNewLines "\r\nLINDSEY\r\n(quietly)\r\nHello, friend.\r\n(loudly)\r\nFriendo!" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ Character (false, true, [Literal ("LINDSEY", new Range(NewLineLength, 7))], new Range(0, 7 + NewLineLength * 2)); Parenthetical ([Literal ("quietly", new Range(8 + NewLineLength * 2, 7))], new Range(7 + NewLineLength * 2, 9 + NewLineLength)); Dialogue ([Literal ("Hello, friend.", new Range(16 + NewLineLength * 3, 14))], new Range(16 + NewLineLength * 3, 14 + NewLineLength)); Parenthetical ([Literal ("loudly", new Range(31 + NewLineLength * 4, 6))], new Range(30 + NewLineLength * 4, 8 + NewLineLength)); Dialogue ([Literal ("Friendo!", new Range(38 + NewLineLength * 5, 8))], new Range(38 + NewLineLength * 5, 8))];

//===== Page Break

[<Test>]
let ``PageBreak - ===`` () =
   let doc = "===" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [PageBreak(new Range(0, 3))]

[<Test>]
// TODO: should this be a synopses? probably, yeah? need clarification from the spec
let ``PageBreak - == (not enough =)`` () =
   let doc = "==" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Synopses ([Literal ("=", new Range(1, 1))], new Range(0, 2))]

[<Test>]
let ``PageBreak - ==========`` () =
   let doc = "==========" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [PageBreak(new Range(0, 10))]

[<Test>]
let ``PageBreak - ======= (with space at end)`` () =
   let doc = "======= " |> FountainDocument.Parse
   doc.Blocks
   |> should equal [PageBreak(new Range(0, 8))]


[<Test>]
let ``PageBreak - ======= blah (fail with other chars)`` () =
   let doc = "======= blah" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Synopses ([Literal ("====== blah", new Range(1, 11))], new Range(0, 12))]


//===== Lyrics

[<Test>]
let ``Lyrics - normal`` () =
   let doc = "~Birdy hop, he do. He hop a long." |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Lyrics ([Literal ("Birdy hop, he do. He hop a long.", new Range(1, 32))], new Range(0, 33))]

[<Test>]
let ``Lyrics - Line break at the end`` () =
   let doc = properNewLines "~Birdy hop, he do. He hop a long.\r\n" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ Lyrics ([Literal ("Birdy hop, he do. He hop a long.", new Range(1, 32)); HardLineBreak(new Range(33, NewLineLength)) ], new Range(0, 33 + NewLineLength)) ]

//===== Line Breaks

[<Test>]
let ``Line Breaks`` () =
   let text = "Murtaugh, springing..." + NewLine(2) + "An explosion of sound..." + NewLine(1) + "As it rises like an avenging angel ..." + NewLine(1) + "Hovers, shattering the air " + NewLine(2) + "Screaming, chaos, frenzy." + NewLine(1) + "Three words that apply to this scene."
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal ([Action (false, [Literal( "Murtaugh, springing...", new Range(0, 22)); HardLineBreak(new Range(22, NewLineLength)); HardLineBreak(new Range(22 + NewLineLength, NewLineLength)); Literal("An explosion of sound...", new Range(22 + 2 * NewLineLength, 24)); HardLineBreak(new Range(46 + 2 * NewLineLength, NewLineLength)); Literal("As it rises like an avenging angel ...", new Range(46 + 3 * NewLineLength, 38)); HardLineBreak(new Range(84 + 3 * NewLineLength, NewLineLength)); Literal("Hovers, shattering the air ", new Range(84 + 4 * NewLineLength, 27)); HardLineBreak(new Range(111 + 4 * NewLineLength, NewLineLength)); HardLineBreak(new Range(111 + 5 * NewLineLength, NewLineLength)); Literal ("Screaming, chaos, frenzy.", new Range(111 + 6 * NewLineLength, 25)); HardLineBreak(new Range(136 + 6 * NewLineLength, NewLineLength)); Literal( "Three words that apply to this scene.", new Range(136 + 7 * NewLineLength, 37))], new Range(0, text.Length))])

//===== Notes
[<Test>]
let ``Notes - Inline`` () =
   let text = "Some text and then a [[bit of a note]]. And some more text."
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Action (false, [Literal ("Some text and then a ", new Range(0, 21)); Note ([Literal( "bit of a note", new Range(23, 13))], new Range(21, 17)); Literal( ". And some more text.", new Range(38, 21))], new Range(0, 59))]

[<Test>]
let ``Notes - Block`` () =
   let doc = "[[It was supposed to be Vietnamese, right?]]" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Action(false, [Note ([Literal ("It was supposed to be Vietnamese, right?", new Range(2, 40))], new Range(0, 44))], new Range(0, 44))]

[<Test>]
let ``Notes - Line breaks`` () =
   let text = properNewLines "His hand is an inch from the receiver when the phone RINGS. Scott pauses for a moment, suspicious for some reason.[[This section needs work.\r\nEither that, or I need coffee.\r\nDefinitely coffee.]] He looks around. Phone ringing."
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Action(false, [Literal ("His hand is an inch from the receiver when the phone RINGS. Scott pauses for a moment, suspicious for some reason.", new Range(0, 114)); Note([Literal("This section needs work.", new Range(116, 24)); HardLineBreak(new Range(140, NewLineLength)); Literal("Either that, or I need coffee.", new Range(140 + NewLineLength, 30)); HardLineBreak(new Range(170 + NewLineLength, NewLineLength)); Literal("Definitely coffee.", new Range(170 + 2 * NewLineLength, 18))], new Range(114, 76 + NewLineLength * 2)); Literal(" He looks around. Phone ringing.", new Range(190 + NewLineLength * 2, 32))], new Range(0, text.Length))]

[<Test>]
let ``Notes - Line breaks with empty line`` () =
   let text = properNewLines "His hand is an inch from the receiver when the phone RINGS. Scott pauses for a moment, suspicious for some reason.[[This section needs work.\r\nEither that, or I need coffee.\r\n  \r\nDefinitely coffee.]] He looks around. Phone ringing."
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Action(false, [Literal ("His hand is an inch from the receiver when the phone RINGS. Scott pauses for a moment, suspicious for some reason.", new Range(0, 114)); Note([Literal("This section needs work.", new Range(116, 24)); HardLineBreak(new Range(140, NewLineLength)); Literal("Either that, or I need coffee.", new Range(140 + NewLineLength, 30)); HardLineBreak(new Range(170 + NewLineLength, NewLineLength)); HardLineBreak(new Range(170 + 2 * NewLineLength, NewLineLength + 2)); Literal("Definitely coffee.", new Range(172 + NewLineLength * 3, 18))], new Range(114, 78 + NewLineLength * 3)); Literal(" He looks around. Phone ringing.", new Range(192 + NewLineLength * 3, 32))], new Range(0, text.Length))]

//===== Span Elements ==============================================================

[<Test>]
let ``Escaped char`` () =
   let doc = "Tom & Jerry" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [Action (false, [Literal ("Tom & Jerry", new Range(0, 11))], new Range(0, 11))]

//===== Indenting

[<Test>]
let ``Scene Heading - Indenting`` () =
   let doc = properNewLines "\r\n\t EXT. BRICK'S PATIO - DAY\r\n\r\nSome Action" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ SceneHeading (false, [Literal ("EXT. BRICK'S PATIO - DAY", new Range(2 + NewLineLength, 24))], new Range(0, 26 + NewLineLength * 3)); Action(false, [Literal ("Some Action", new Range(26 + NewLineLength * 3, 11))], new Range(26 + NewLineLength * 3, 11))]

[<Test>]
let ``Character - Indenting`` () =
   // white spaces have to be ignored
   let doc = properNewLines "\r\n\t   LINDSEY" |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ Character (false, true, [Literal ("LINDSEY", new Range(NewLineLength + 4, 7))], new Range(0, 11 + NewLineLength))]

[<Test>]
let ``Dialogue - Indenting`` () =
   let doc = properNewLines "\r\n\t  LINDSEY\r\n   \t Hello, friend." |> FountainDocument.Parse
   doc.Blocks
   |> should equal [ Character (false, true, [Literal ("LINDSEY", new Range(NewLineLength + 3, 7))], new Range(0, 10 + NewLineLength * 2)); Dialogue ([Literal ("Hello, friend.", new Range(15 + NewLineLength * 2, 14))], new Range(10 + NewLineLength * 2, 19))]

[<Test>]
let ``Action - indenting`` () =
   let text = properNewLines "\tNatalie looks around at the group, TIM, ROGER, NATE, and VEEK.\r\n\r\n\t\tTIM, is smiling broadly."
   let doc = text |> FountainDocument.Parse
   doc.Blocks
   |> should equal  [Action (false, [Literal ("\tNatalie looks around at the group, TIM, ROGER, NATE, and VEEK.", new Range(0, 63)); HardLineBreak(new Range(63, NewLineLength)); HardLineBreak(new Range(63 + NewLineLength, NewLineLength)); Literal ("\t\tTIM, is smiling broadly.", new Range(63 + 2 * NewLineLength, 26))], new Range(0, text.Length))]
