namespace BuildIndicatron.Core.Helpers
{
	public class RandomTextHelper
	{
		#region All Text

		private static string[] _oneLiners = new[]
			{
				"When did Anakin's Jedi masters know he was leaning towards the dark side?. In the Sith Grade ",
				"Why do Doctors make the best Jedi?. Because a Jedi must have patience ",
				"How is Ducktape like the Force?. It has a Dark Side, a Light side and it binds the galaxy together ",
				"What do you call a Sith who won't fight?. A Sithy ",
				"Where does Princess Leia go shopping for clothing and such?. At the Darth Maul, of course ",
				"What do you call 5 siths piled on top of a lightsaber?. A Sith-Kabob! ",
				"Which Star Wars character uses meat for a weapon instead of a Lightsaber?. Obi Wan Baloney ",
				"Why is a droid mechanic never lonely?. Because he's always making new friends! ",
				"What did the rancor say after he ate a Wookiee?. Chewie! ",
				"What do Gungans put things in?. Jar Jars ",
				"Why did Yoda visit Bank of America yesterday?. He needed a bank clone! (Loan) ",
				"Why does Princess Leia keep her hair tied up in buns?. So it doesn't Hang Solow! ",
				"Why didn't Luke Skywalker cross the road?. Because he got a ticket for Skywalking ",
				"What does Yoda say to encourage a Padawan before a test?. Do well, you will do! ",
				"Why does Leia wear buns on her head?. In case she gets hungry in a Senate meeting ",
				"How many stormtroopers does it take to replace a lightbulb?. Two; one to screw the bulb in, the other to shoot him and take the credit "
				,
				"What side of an Ewok has the most hair?. The outside ",
				"Who tries to be a Jedi?. Obi-Wannabe ",
				"Which Star Wars character works at a restaurant?. Darth Waiter ",
				"What do you call a female Mandalorian?. A Womandalorian ",
				"What do Whipids say when they kiss?. Ouch ",
				"Why did the Stormtrooper start jumping up and down?. He stepped on Ant-hillies ",
				"What do Star Destroyers wear to parties?. A bow TIE ",
				"Why is Han Solo a loner?. Because he's solo ",
				"Where does Jabba the Hutt eat?. Pizza Hutt ",
				"Why did the crazy Angrallian Toobir cross the nebula?. To get to the other dementia ",
				"Why did the smuggler cross the spacelanes?. To get to the other side ",
				"What's the differance between an ATAT and a stormtrooper?. One's an Imperial walker and the other is a walking Imperial "
				,
				"How many Sith does it take to screw in a hyperdrive?. Two, but I don't know how they got in it ",
				"What goes, \"Ha, ha, ha, haaaa.... AGGGHHHH! Thump\"?. An Imperial Officer laughing at Darth Vader ",
				"Why did Yoda cross the road?. Because the chickens Forced him to ",
				"As a Disney character what song would Vader sing?. \"When You Wish Upon A Death Star\" ",
				"Why did the Ewok fall out of the tree?. It was dead ",
				"Where does Qui-Gon keep his jam?. In a Jar-Jar ",
				"What is Jabba the Hutt's middle name?. \"The\" ",
				"Why is the Millenium Falcon so slow?. Because it takes a millenium to go anywhere ",
				"What is a jedi's favorite toy?. A yo-yoda ",
				"Why should you never tell jokes on the Falcon?. The ship might crack up ",
				"What happens when a red and white X-Wing crashes into green water?. It gets wet ",
				"Why do Twi'leks like to flip coins?. So that they can say, \"Heads or tails!\" ",
				"What time is it when an AT-AT steps on your chronometer?. Time to get a new chronometer ",
				"Why is a droid mechanic never lonely?. Because he's always making new friends ",
				"What do Jawa's have that no other creature in the galaxy has?. Baby Jawas ",
				"What do you call a person who brings a rancor its dinner?. The appetizer ",
				"Why do vornksrs stop slowly?. They're afraid of whiplash ",
				"What's the name of the worst cantina on Coruscant?. The Ackbar ",
				"How would a fat Rogue get into his X-wing?. He'd Wedge himself in ",
				"How many Corellians does it take to change a glowpanel?. None, if the room's dark, then you can't see them cheat at sabacc"
				,
				"Luke and Obi-Wan are in a Chinise restaurant and Luke's having trouble Finally, Obi-Wan says, \"Use the forks, Luke\""
			};

		private static string[] _insult = new[] {"Your birth certificate is an apology letter from the condom factory.",
			"You must have been born on a highway because that's where most accidents happen.",
			"I don't exactly hate you, but if you were on fire and I had water, I'd drink it.",
			"You are proof that God has a sense of humor.",
			"Shut up, you'll never be the man your mother is.",
			"Hey, you have somthing on your chin... no, the 3rd one down",
			"Do you wanna lose ten pounds of ugly fat? Cut off your head",
			"Am I getting smart with you? How would you know?",
			"Your family tree must be a cactus because everybody on it is a prick.",
			"I'd like to see things from your point of view but I can't seem to get my head that far up my ass.",
			"It's better to let someone think you are an Idiot than to open your mouth and prove it.",
			"You are so stupid, you'd trip over a cordless phone.",
			"You are so old, your birth-certificate expired.",
			"Well I could agree with you, but then we'd both be wrong.",
			"So, a thought crossed your mind? Must have been a long and lonely journey.",
			"Why don't you slip into something more comfortable -- like a coma.",
			"If assholes could fly, this place would be an airport!",
			"Are you always an idiot, or just when I'm around?",
			"You stare at frozen juice cans because they say, concentrate.",
			"Come again when you can't stay quite so long.",
			"You may not be the best looking girl here, but beauty is only a light switch away!",
			"It looks like your face caught on fire and someone tried to put it out with a hammer.",
			"I love what you've done with your hair. How do you get it to come out of the nostrils like that?",
			"You do realize makeup isn't going to fix your stupidity?",
			"Ordinarily people live and learn. You just live.",
			"Looks like you traded in your neck for an extra chin!",
			"Don't you need a license to be that ugly?",
			"You occasionally stumble over the truth, but you quickly pick yourself up and carry on as if nothing happened.",
			"I've seen people like you, but I had to pay admission!",
			"Jesus loves you, everyone else thinks you're an asshole!",
			"Shock me, say something intelligent.",
			"Don't feel sad, don't feel blue, Frankenstein was ugly too.",
			"Aww, it's so cute when you try to talk about things you don't understand.",
			"You are proof that evolution CAN go in reverse.",
			"You are so old, you fart dust.",
			"I may be fat, but you're ugly, and I can lose weight.",
			"I heard you took an IQ test and they said you're results were negative.",
			"If I want your opinion, I'll give it to you.",
			"I wish you no harm, but it would have been much better if you had never lived.",
			"When was the last time you could see your whole body in the mirror?",
			"If a crackhead saw you, he'd think he needs to go on a diet.",
			"Being around you is like having a cancer of the soul.",
			"Even if you were twice as smart, you'd still be stupid!",
			"We all sprang from apes, but you didn't spring far enough.",
			"Learn from your parents' mistakes - use birth control!",
			"If what you don't know can't hurt you, you're invulnerable.",
			"Do you still love nature, despite what it did to you?",
			"Your parents hated you so much you bath toys were an iron and a toaster",
			"You're as useless as a screen door on a submarine.",
			"Maybe if you ate some of that makeup you could be pretty on the inside.",
			"I'd like to help you out. Which way did you come in?",
			"If you had another brain, it would be lonely.",
			"I don't know what makes you so stupid, but it really works!",
			"You must think you're strong, but you only smell strong.",
			"The best part of you is still running down your old mans leg.",
			"100,000 sperm, you were the fastest?",
			"Yeah you're pretty, pretty stupid",
			"Nice tan, orange is my favorite color.",
			"Is your name Maple Syrup? It should be, you sap.",
			"Brains aren't everything. In your case they're nothing.",
			"I look into your eyes and get the feeling someone else is driving.",
			"If you spoke your mind, you'd be speechless.",
			"Ever since I saw you in your family tree, I've wanted to cut it down.",
			"Just reminding u there is a very fine line between hobby and mental illness.",
			"Your mom must have a really loud bark!",
			"Are your parents siblings?",
			"You're as useful as an ashtray on a motorcycle.",
			"You act like your arrogance is a virtue.",
			"Beauty is skin deep, but ugly is to the bone.",
			"You're the reason why women earn 75 cents to the dollar.",
			"When anorexics see you, they think they need to go on a diet.",
			"I hear the only place you're ever invited is outside.",
			"Please tell me you don't home-school your kids.",
			"You'll make a great first wife some day.",
			"You are so old, even your memory is in black and white.",
			"For those who never forget a face, you are an exception.",
			"People like you are the reason I work out.",
			"You're stupid because you're blonde."};

		private static readonly string[] _quotes = new[] {"SendQuotesI find your lack of faith disturbing.", "You don't know the power of the dark side!",
			"Luke, I am your father!.",
			"Today will be a day long remembered. It has seen the death of Kenobi, and soon the fall of the rebellion.",
			"The force is strong with this one.", "I sense something, a presence I've not felt since.......",
			"You should not have come back!",
			"The ability to destroy a planet is insignificant next to the power of the force.",
			"Just for once, let me look at your face with my own eyes.",
			"I've been waiting for you, Obi-wan. We meet again at last. The circle is now complete. When I left you I was but the learner. Now I am the master.",
			"Perhaps I can find new ways to motivate them.", "Obi-Wan has taught you well.",
			"Obi-Wan once thought as you do. You don't know the power of the Dark Side, I must obey my master.",
			"It is too late for me, son. The Emperor will show you the true nature of the Force. He is your master now.",
			"You are unwise to lower your defenses!", " As you wish.",
			"No. Leave them to me. I will deal with them myself.", "My son is with them.",
			"You cannot hide forever, Luke.", "Don't fail me again, Admiral.",
			"Asteroids do not concern me, Admiral. I want that ship, not excuses.",
			"He will join us or die, my master.",
			"Alert all commands. Calculate every possible destination along their last known trajectory.",
			"Impressive. Most impressive. Obi-Wan has taught you well. You have controlled your fear. Now, release your anger. Only your hatred can destroy me.",
			"The force is with you, young Skywalker, but you are not a Jedi yet.",
			"What is thy bidding, my master?", "When I left you I was but the learner. Now I am the master."};

        private static string[] _greetings = new[] {"Hi",
            "Good morning",
            "Morning!",
            "Hey",
            "What's up?",
            "Sup?",
            "How's it going?",
            "Howdy",
            "Well hello!",
            "Why hello there.",
            "Yo.",
            "Greetings!",
            "Look who it is!",
            "Look what the cat dragged in!",
            };

	    #endregion

		public static string Quotes
		{
			get
			{
				return (_quotes).Random();
			}
		}

		public static string Insult
		{
			get
			{
				return (_insult).Random();
			}
		}

		public static string OneLiner
		{
			get
			{
				return (_oneLiners).Random();
			}
		}
        
        public static string Greetings
		{
			get
			{
				return (_greetings).Random();
			}
		}
	}
}