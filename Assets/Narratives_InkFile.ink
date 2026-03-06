//Emotion Var
//0=calm, 1=medium,2=anxious for example
VAR Anxiety=0
VAR Sadness=0
VAR Repulsion=0

// Ending Var
VAR Dream=0
VAR Achievement=0
VAR Stability=0
VAR Friend=0

// Dream Index:Dream of light (DOL), Dream of research (DOR), Dream of travel (DOT), Dream of story (DOS)
VAR DOL=0
VAR DOR=0
VAR DOT=0
VAR DOS=0

// FoodSong Var
VAR Dessert=""
VAR Song=""
VAR Drink=""

// NPC Affinity
VAR TeenAffinity=0
VAR AdultAffinity=0
VAR KidAffinity=0

// External Function, defines a var here, send to C# function and get a response back to inkle
EXTERNAL EasterEggTrigger(ID)
->Opening

===Opening===
//Opening section intends to be created on a black background (like a terminal), Electronic Highway Sign font, typewriting animation
Hey, can you see me?#type_animation
It's been a long time since we last met.#type_animation
How are you?#type_animation
Who are you?#type_animation #Class:Blue
I...I'm not sure.#type_animation
Perhaps I'm YOU. Your 15-year-old self.#type_animation

->Menu
//First day the food choice is fixed: must choose instant noodle, coffee, an intense music
===Menu===
A Piece of Night
->Start

===Start===
How are you?#Teen_smile
Before answering that, I should prompt you for an explanation of your existence first.#Teen_smile
Errr, you are so boring and serious, and always view the world with an epistemological approach. #Teen_speechless
Are you speaking of yourself since you CLAIM to be my 15-year-old self, or I should call you Maggie?#Teen_default
Maggie: I'm nothing like you. But fair enough, that's how my friends call me. Do you remember Patty? Do you still have contact with her?#Teen_default
Kind of, if you call that regular New Year wishes broadcasted to everyone is a form of contact. #Teen_default
Growing up is just so dull. What are you doing now?#Teen_default
I'm a light engineer in the Light City, making sure a region is fully covered by light sources.#Teen_default
Ever since we built the Dyson swarm, solar energy can be transmitted directly. Now every city is building full light coverage systems.#Teen_default
Maggie: For what? #Teen_default
* Pragmatically feasible. Better for surveillance and security.#Teen_default
Maggie: Such a dry reasoning. Sounds like what I will write for my argumentative essay.#Teen_speechless
~Stability++
->Combine1
* A fully lit world is just a beautiful vision.#Teen_default
->DOL1

=DOL1
Maggie: Like achieving a straight A on your transcript? #Teen_default
*Totally.#Teen_default
~Achievement++
->Combine1
{Sadness==0:
*Not Exactly. More like my dream as always, to create a world with no darkness, conspiracies and evil. #Teen_default
~Dream++
~DOL++
->Combine1
}
{Repulsion==0:
*Don't you think energy tech itself is fascinating? #Teen_default
~Dream++
~DOR++
->Combine1
}

* I don't know. #Teen_default
Maggie: OK, then we are on the same page: I don't know what I'm doing, and really cannot imagine at the age of 30 I will still be like this.#Teen_default
->IDK1

=IDK1
* The "Not knowing" state in your 15s and in my 30s is not the same.#Teen_default
One is not knowing what you want, the other is not knowing how to choose.#Teen_default
Maggie: If your life is a balance with two sides, your job is on one side, what will be your other side? #Teen_default
{Repulsion==3 && Sadness==3: 
* Not sure, but I need to escape from my current life.#Teen_default
~Stability--
->Combine1
}

* Yeah, you're right. People never know the cutting point of growing up.#Teen_default
->Combine1

=Combine1
Maggie: Anyways, how are you? #Teen_default
It's the third time you are asking this question.#Class:Blue
Maggie: And NEVER get a proper answer. As a 15-year-old teenager I'm really curious about my future life.#Teen_default
I have told you, you want me to repeat? I'm a light engineer...#Class:Blue
Maggie: Urgggg not this one. I want to know about your LIFE. Come home alone, grab that instant noodle and a 10PM cup of coffee, play a sound track that almost explodes my brain. Is that your life?#Teen_speechless
Not everyday, there's an emergency. The area under me encounters a bug and I need to resolve it before the new year's deadline. #Teen_default
This will affect our year-end assessment as a department, and I'm unfortunately on the year-end call. #Class:Blue#Teen_default
My boss is really pushing hard. So I have to stay up to monitor and fix the real time issues. #Class:Blue#Teen_default
Maggie: Am I wasting your time then?#Teen_default
You know that and you are still asking. #Class:Blue#Teen_default
Maggie: Great to hear, I love wasting people's time, just like how I always waste Patty's precious studying hours. #Teen_smile
(Naughty girl...)#Class:Purple#Teen_default
Maggie: So what's your call tonight? #Teen_default
I fixed a few spots' coverage - great, my boss praised me for that. #Class:Blue#Teen_default
Left with one, a deserted train station at the Riverside district. #Class:Blue#Teen_default
The light network has definitely reached, but looks as if this specific place is resisting light from coming in.#Class:Blue#Teen_default
Moreover, detection system is completely lost there, so indeed my boss knows nothing about it.#Class:Blue#Teen_default
Maggie: A deserted train station... That's not how we call the Riverside Station.#Teen_default 
Maggie: Only this old station willingly accommodate those old-fashioned trains between Light City and my hometown.#Teen_sad
Now that line is also obsoleted. #Class:Blue#Teen_default
Maggie: That's really sad. I'm sorry for your loss.#Teen_default
I really didn't lose anything. Indeed, the train system has extinguished in the entire country.#Class:Blue #Teen_default
Most of them are requisited to build city facilities, such as tairports and Helio Centers.#Class:Blue #Teen_default
Maggie: You are really talking like my chemistry teacher. #Teen_default
What?#Teen_default
Maggie: Speaking a language that I understand yet never really comprehend as if no explanation is needed.#Teen_speechless
* That's my intention. You don't need to know any of these terms in detail.#Class:Blue #Teen_default
Maggie: You should know better than anyone that I don't like to be treated as a kid.#Teen_anger
~TeenAffinity--
->Explanation
* I'm sorry, I should have explained to you.#Class:Blue #Teen_default
~TeenAffinity++
->Explanation

=Explanation
Anyways. Tairports, basically is the airtaxi ports.#Teen_default #Teen_default
No more taxis nowadays. You can just take autonomous flights wherever you go.#Class:Blue #Teen_default
And Helio Centres are like Tokamak reactors in the past, huge, dangerous, high tech, and supplies endless energy, but this time the energy is directly extracted from the sun.#Class:Blue #Teen_default
Maggie: OK, I'll buy that. At least you are more patient than the chem teacher.#Teen_default
Maggie: Luckily, I haven't grown up into a mean adult. #Teen_smile
* NO NO NO, I have the meanest manner behind the scene (make a harsh face)#Teen_default
~TeenAffinity++
->AfterTease
Maggie:(Laugh out loud) Woah looks like I will be scared! #Teen_smile
* Whatever.
->AfterTease

=AfterTease
Maggie: What are the consequences? I mean, if you fail to report.#Teen_default
If you really think about it - ya, nothing is really serious. Worst case scenario, losing my job. #Class:Blue #Teen_default
But most likely, no one will notice.#Class:Blue #Teen_default
Detection is missing. We move on. The city moves on.#Class:Blue #Teen_default
Maggie: Sounds nothing like an emergency.#Teen_default
Rationally yes. This detection issue is quite common around the system, and no effective solution for now. #Class:Blue #Teen_default
Even if someone finds it, I will just be one of the many cases, since I've already paid my dues elsewhere.#Class:Blue #Teen_default
BUT, if I report and fix it before New Year, I definitely will secure a performance-based early promotion.#Class:Blue #Teen_default
Maggie: That's attractive. Apart from higher pay, what will you get?#Teen_default
Longer holidays, fully remote working - plus an exchange program in Star City with all costs covered by my company. I've been craving for their chicken rice for years. #Class:Blue #Teen_default
{Repulsion==3:
Maggie: But you sound reluctant.
*Am I? I don't think so.#Class:Blue #Teen_default
Maggie: OK, then just assume I said nothing. #Teen_default
->Benefits
*......(Silence)#Class:Blue #Teen_default
Maybe the benefits need me more than I need them.#Teen_default
~Stability--
~Achievement--
->Benefits
}
{Repulsion==0&&Sadness==0:
Maggie: You sound excited. Does the vision of success excite you that much?#Teen_smile
* Yes! I'm super happy when thinking about what I can do with the higher pay.#Class:Blue #Teen_default
I may rent a single person apartment in Star City, facing the river and see the starry city on the other side. That's exactly the life I'm looking for. That's my goal of this everlasting migration - to find a home of my own.#Class:Blue #Teen_default
~Achievement++
->Benefits
* Of course! Star City has the most advanced technology. #Class:Blue #Teen_default
Apart from their light technology, I'd love to see their space tourism systems.#Class:Blue #Teen_default
Because they have much trading transportation for those crops planted on other planets, they have in the meantime developed the mature space transit that only needs a few thousand bucks.#Class:Blue #Teen_default
~Dream++
~DOL++
->Benefits
* Hmmm, indeed, just the vision of travelling to a new place excites me.#Class:Blue #Teen_default
Maggie: But you can travel whenever you want!#Teen_default
Looking at my duty. How can I leave all this behind?#Class:Blue #Teen_default
Maggie: You can resign. I'm serious.#Teen_default
And you will pay for my rents? Also my costs of travelling around?#Class:Blue #Teen_default
Maggie: No way! I'm still a little girl counting every penny just to have some savings.#Teen_smile
What are you saving for?#Class:Blue #Teen_default
Maggie: A trip to Star City. Patty has promised me, if she manages to get an offer from Light City University, her parents will reward her some money. Plus her private savings, that may be enough for a trip to Star City.#Teen_smile
I remember that. Vaguely.#Class:Blue #Teen_smile
Maggie: What's the outcome?#Teen_smile
    {TeenAffinity>0:
    (She was asking in an eager manner. I really don't want to disappoint her.)#Class:Purple#Teen_smile
    I prefer not to tell.#Class:Blue #Teen_smile
    Maggie: OK. But anyways, you are going to Star City.#Teen_default
    - else:
    We didn't make it.#Class:Blue #Teen_default
    Maggie: Hmmm. Expected... #Teen_sad 
    Maggie: I've thought of that, really. I didn't really think it will happen all this time.#Teen_sad 
    Maggie: I feel like I'm just saving money for that tiny promise of escape,#Teen_sad 
    Maggie: so that I'm still holding on to something solid in this strange city that never quite feels like mine.#Teen_sad 
    Maggie: But now, I think there's no longer a need for saving.#Teen_sad 
    ~DOT--
    ~Dream--
    }
->Benefits
}
->Benefits

=Benefits
Maggie: Another exchange program... We are always exchanging, physically and culturally.#Teen_sad
I can never forget the day you received a call about your successful application for a sponsored exchange to Light City.#Class:Blue #Teen_sad 
They called in multiple times when you were watching a movie with Patty. You guys took out all pocket money to buy yourselves one reckless day., and those phone calls came in such a frustrating way during the movie.#Class:Blue #Teen_sad 
You pushed that hang-up button hard to punish it for interrupting the movie. It went silent. After that two hours, you received the call again, answered "Yes" to the offer, and burst into tears in Patty's embrace.#Class:Blue #Teen_sad 
Maggie: From my perspective, that was the most courageous moment in my life. How do you view it after 15 years?#Teen_smile 
(Chuckling) Courageous, what a word. Bold choice, I would say. You only thought you knew what you were stepping into.#Class:Blue #Teen_default
Maggie: Light City has a very different rhythm from River Ville. Back home, you were outstanding. In Light City, you were simply… one of many. And I truly miss Patty.#Teen_sad
{Sadness==3:
(Nostalgia settles quietly, like household lights slowly claiming the dark. You have not thought about Patty in a long time, but at that moment, you see her pimpled face and that typical smile with braces. )#Class:Purple #Teen_default
(Her dark eyes meet yours. You recognize that loneliness.#Class:Purple #Teen_default
(It is the same one that used to stare back at you in the boarding school hostel.)#Class:Purple #Teen_default
I almost forgot her face and what we have been through. I've grown so used to being alone.#Class:Blue #Teen_default
Maggie:You should go back and contact her some day. Now I cannot really call her that much, she's studying at a high school known for its strict, militarized discipline, so she really has limited access to her phone.#Teen_default
Maggie: Whenever the weekend comes, the first thing she does is to get back her phone and reply my random messages, I reply immediately, just to spend that precious hour of freedom with her.#Teen_smile
Maggie: "I'm striving to get to the same college as you. By then, I will have my phone with me, and have you every day!" That's what Patty said. Now you have your phone, why not give her a call?
* It feels so embarassing to contact a person that you have not talked to for a long time.#Class:Blue #Teen_default
Maggie: Ah, that's fair. Perhaps I should cherish that time before our friendship fades away.#Teen_default
->AfterFriend
* I'll try that, after this year-end catastrophe is over.#Class:Blue #Teen_default
Maggie: I bet you will.#Teen-smile
~Friend++
->AfterFriend
}
->AfterFriend

=AfterFriend
But look at me. I got it through. You've made it. I have settled down in this city. This will also be your future.#Class:Blue #Teen_default
Maggie: This really doesn't sound promising, if growing up and settling down means overworking until 11PM. #Teen_speechless
Thinking on the bright side: this IS your current life. We are used to this routine.#Class:Blue #Teen_default
Maggie: But I worked so hard, not for you to continue that routine! If I know I'm working for your hopeless future, I just don't give a shit - grades, competitions, rankings...#Teen_angry
Maggie: Those nights I cried for a failure to enter Olympiad finals. I put in all my efforts for the study hours. What's the meaning of all these trifles?#Teen_angry
30 is not end of the world. There's still more unknowns ahead.#Class:Blue #Teen_default
Maggie: What does turning to 30s mean? #Teen_default
It doesn't really mean anything, unless you give meaning to it. #Teen_default 
Speaking of that, what do you think of your 30s if you never meet me?#Class:Blue #Teen_default
Maggie: I was dreaming of a satisfying life, and scared of finding out that things never turn out as what I expect. #Teen_default
This statement still holds today when I think about my 40s. We are always getting a "fear" advance to prevent ourselves from falling behind, just as if any cash advance is going to solve our current problems.#Class:Blue#Teen_default
We keep borrowing fear from the future, thinking it will protect us.But one day you realize, you're already living in the future you once tried to guard against.#Class:Blue#Teen_default
(Suddenly, the screen in front of you turns dark. Your work might have gone unsaved. Damn it - You don't really understand why in an age of constant energy supply, there is still no effective way to resolve computer blackout)#Class:Purple #Teen_default
(Maggie's voice disperse from the space, as if she never exists. You sigh, leave your seat to grab a cup of hot milk that will help you get to sleep, but suddenly all the lights in your room are off.)#Class:Purple 
(You room has fallen into darkness, or it has brought you back to the nights before time.)#Class:Purple #Picture_Room2 
(Those nights belong to the city once, but now are rejected as an error ever since the establishment of Helio centres.)#Class:Purple #Picture_Room1
(You have no other choice but to look outside of your window. Lights are on as normal, rooms are covered under the curtain. You suddenly feel that you are lost in the city, or the city has left you behind.)#Class:Purple#Picture_CityLight
(This recollects part of your memory as a student in a foreign city. Back in River Ville, one of your night activities is to drag two folding chairs to the backyard, knock Patty's door. You will sit together, shoulder beside shoulder, trying to figure out constellations in the sky.)#Class:Purple
(When you came to the Light City, the first thing you noticed and felt sad about it...You once forgot such a minor detail, and that lost feeling suddenly comes back to you like a rising tide.#Class:Purple
(You never saw a single star in the Light City. )#Class:Purple
(You were once sad about it, but you ended up getting used to it.)#Class:Purple
*Check the Breaker and on the light. You still have work to do. #Class:Blue
You found a torch that has not been used for a long time. You crept out of your room, and fixed the issue in breaker - fixing utilities is just an essential skill to make a living alone for all these years.#Class:Blue
Everything has gone back to normal. Luckily, your laptop has restarted, and your progress was autosaved to the latest checkpoint. #Class:Blue
You didn't see or hear Maggie anywhere in the space. Just in one minute after you go back to work, in that intense brainstorming for bug-fixing, you have already forgotten everything about Maggie and your past.
~Achievement++
->Day2
*{Sadness==3}Stay in the darkness.
??:Hey, can you hear me?
(You looked into your laptop. The screen is dark, but the voice undoubtedly comes from there.)#Class:Purple
??:It's me.
You still there, Maggie?#Class:Blue
Maggie: Yes.
(Her face gradually appears on your screen, from pixelated points to a clear figure. )#Class:Purple #Teen_default
(Like rendering a scene when you reach a new place in game. Strange.)#Class:Purple #Teen_default
Maggie: Power cut on my side, but tomorrow is my chem finals. So frustrating.#Teen_speechless
What should I do? Or what did you do?#Teen_default
(What did you do? that question comes to your mind. You definitely remember that night of power cut. That was a major incident in the Light City, and has been on the top1 popularity in social media discussion tags for consecutive weeks.)#Class:Purple #Teen_default
(Everything was shut down suddenly. The city experienced drastic turmoil in the night.Bank systems collapsed, lots of embezzling attempts, and multiple car accidents occurred as the cars energy was also connected to the city's centralized supply.)#Class:Purple #Teen_default
(Critiques to the system surge. Key opinion leaders pinpoints the complacency of system: when energy becomes unlimited, storage techs are obsoleted with ignorance, as if there will never be any possibility of systematic collapse)#Class:Purple #Teen_default
(But you would never know about these that night. Or you won't care about it. What you care about, is only...)#Class：Purple #Teen_default
->Stars

=Stars
*[Your chemistry exam.]->Chemistry 
#Class:Blue #Teen_default
*[The stars in the sky.]->StarrySky
#Class:Blue #Teen_default

=Chemistry
I revised my chem exam that evening for the last sprint, and passed with flying colors.#Class:Blue#Teen_default
Maggie: But how?#Teen_smile
Try to dig out your torch from the suitcase. I put everything I don't normally use in the suitcase. You should know, that suitcase is really occupying lots of space yet our hostel room is so tiny, so I used it as a storage.#Class:Blue#Teen_default
Maggie: Oh, thanks for your reminder. I'll do that!#Teen_smile
(She disappears from your screen, leaving no trace behind. At that moment, your room reconnects to the city and your laptop is on. Your work was autosaved to the latest checkpoint, fortunately.)#Class:Purple
(You take a deep breath, and start typing on your screen. Cursor is blinking and moving quickly, as if those codes flow out from your instinct without any thinking process.)#Class:Purple
*"For the greater goods." You mumbled.#Class:Blue 
(Light up the city is always your dream, isn't it? )#Class:Purple 
(You once witnessed children mysteriously missing in that undeveloped River Ville: for undesired gender, poverty, or just that the parents are never properly prepared to nurture a child. )#Class:Purple
(You once saw a red knitted hat alongside the river, soaked and dirty, sized to accommodate a baby's head. It was just in a second that you recall the lady living opposite was knitting it in the front yard. )#Class:Purple 
(You returned it to her, and she burst into tears. It was then you know she has lost her daughter for three days, and no one besides her was actively seeking.)#Class:Purple
(Deliberate ignorance or a murder? You will never know. But that constructs your belief of using your knowledge to build a lighted world: Illuminate every corner where evil breeds, so that all darkness is exposed, and nothing wicked can remain hidden.)#Class:Purple
~Dream++
~DOL=DOL+3
~DOR++
->Day2
*"For a better life." You mumbled.#Class:Blue
(As if your achieving in life all this while. You set your goal, follow your plans step by step, and you will attain it one day, just a matter of time.)#Class:Purple
(You will sleep at 2:00AM tonight as you always do. Tomorrow you will wake up at 7:30AM to catch the shuttle to your office. If you complete this feature on time, you will be freed from that unnecessary commitment of attendance, and you will  wake up at 10:00AM each day if there is nothing urgent.)#Class:Purple
(That's why you are always working so hard, from teenage years till now. It is 29 Dec, 3 days to new year. You still have ample time to work.)
~Stability=Stability+2

->Day2
*"To make my family proud of me." You mumbled.
(You have been walking alone for so long, and you know your parents are proud of you. As the only successful applicant from River Ville, your name is equated to a legend of success in your hometown.)#Class:Purple
(Your parents became celebrities in the ville. Shop owners round down their bill to exchange for educational advice, villagers gift them fruits to visit your past bedroom and read your study notes)#Class:Purple
(But they have all these benefits at the cost of losing you. You seldom look back to River Ville, spend all your time to work in the city during holidays, so that you can repay to your family.)#Class:Purple
(Sadly, your parents have passed away last year due to pandemic, and you never managed to go back. The last reason for you to look back has dissipated in time.)#Class:Purple
(But you still remember the last message from your mother.)#Class:Purple
("We are always proud of you.")#Class:Purple#Picture:PhoneInterfacewithWords
(For them to be proud of you. That was your goal before, and will last forever. Your parents raised you up to escape from your destiny, and escape from themselves, and you have to live a good life to repay their sacrifice.)#Class:Purple
(You live your life as a monument, so that it may commemorate the obscurity of their unnamed lives.)#Class:Purple
(That's too far. You have wasted so much time in thinking. Tomorrow you still need to wake up at 7:00AM to catch the first shuttle. There is a government agent visiting the helio centre, you have to get ready.)#Class:Purple
~Achievement=Achievement+3
->Day2


=StarrySky
Are you staying at the rooftop study area?#Class:Blue #Teen_default
Maggie: Ya, you know me. I love that place, though it's too windy to keep my notes in control. #Teen_default
Look up.#Class:Blue #Teen_default
(Maggie's figure disappears from your screen. In a short while, you see stars emerging on your screen. You see the stars through Maggie's eyes. #Picture:StarryNight
//Or can we use Van Gogh's starry night as a CG here? Do not use the most famous one, but some other starry nights he drew?
(Everything including time has paused. You immerse in the darkness, feeling like home.)#Class:Purple 
(You cannot exactly remember the outcome of that chemistry test. And standing at this point of time, everything that once mattered doesn't matter to you anyway.)#Class:Purple
(What about your work? You try to imagine yourself at the age of 40. Will you regret for not fixing that error on time, which defers your promotion at every step; or you will regret for not looking up to the starry sky that night?)#Class:Purple
(Everything has returned to darkness and solitude. In that stagnant silence, you have a faint glimpse of your calendar: it is 29 Dec.)#Class:Purple 
(You still have three days to report to your boss, and that determines whether you are able to secure the earliest round of merit-based promotion next year.) #Class:Purple
(However, a sudden ideal struck you in your mind: is there anything more important, more compelling to decide apart from work?)#Class:Purple
(You have a vague sense of it, but you know you will still turn on your laptop shortly, work until 2AM, and get up at 7:30AM to catch the office shuttle tomorrow.)#Class:Purple
~Dream++
~DOT++
->Day2


===Day2===
An official visited the Helio Centre today. It took you far too long to explain why he had to change into a radiation protection suit, and why no phones were allowed inside the core tower.#type_animation
He is nitpicking your service. You are trained to stabilize light cores, not to soothe fragile egos.#type_animation
That visit has drained your energy. When you come home, you only want to grab yourself some food. Perhaps hot chocolate and cheese cake can be a good choice? Definitely not 10PM coffee and instant noodle. You need to treat yourself.#type_animation
->AfterSelection

=AfterSelection
??:Hi Hi, can you hear me?
Again???#Class:Blue
(You can't tell why such things happen again. It seems like something supernatural is going on. Though you definitely feel that these encounters are wasting your time - yesterday evening, to complete your plan, you stayed up until 3AM, 1hr later than you normal routine.)#Class:Purple
(It's such a bad day. You almost hope there are two toothpicks there to hold your eyelids open, so that you won't be in that sleepy state when presenting to that frustrating man.)#Class:Purple
(On your way back, you keep telling yourself: Do not answer. Not a word. Even if that voice is calling you. Pretend it never exists.)#Class:Purple
??:Hey, are you there?
(That voice keeps calling. You turn on your laptop, and there's no figure showing. It would be easier to pretend the voice doesn’t exist.)
*[Open your work file and proceed with your plan today.]->Work
*[Respond to the voice.]->Respond

=Work
??:Catch me if you can!
(You need to focus. Your progress has been delayed yesterday, and you can't afford to drag behind another day.)#Class:Purple
*{Repulsion==3}"Get away from here."You said.
(You said that with a decisive tone, leaving no room for question. You didn't hear anything after then, as if that existence has been erased.)#Class:Purple
->AfterWork

*Pretend you never hear anything.
??: I think I've been giving you sufficient clues to find me - guess where I am? (Deliberately lowered voice) Agent X is in the midst of investigating a evil dryad. This dryad, pretends to be dead and swallows everything that approaches... Help me! Agent X is caught!
->Pretend1

=Pretend1
*[Pretend you never hear anything]->Pretend

*[I know where you are! You are inside the body of Bernard!]->Respond
#Class:Blue

=Pretend
??: Come on! What should I do to get you respond? I know you are here. I always know.
This is the final notice: if you don't respond, I will never talk to you again.
*[Pretend you never hear anything]->AfterWork
*[Ok, OK, I hear you. You must be with poor old Bernard]->Respond

=Respond

(Bernard is an old oak tree. Every year towards winter, you would see squirrels revolving around.)#Class:Purple
(Bernard has a hollow body, and down at the root there is an entrance that can accommodate a child. That was your secret base. You didn't even tell Patty about that, so you are always the winner in hide and seek. It was the only place in the world that belonged solely to you.)#Class:Purple
??:Amazing!
(A girl emerges on your screen. Welcome, Agent X, to future - yes, Agent X is your 6-year-old self. She's an agent from supernatural research institution - based on her own setting)#Class:Purple #Kid_default
(Why did you call yourself Agent X? You don't know, but such a cringe worthy past!)
Agent X: Tell me more about yourself. You are the first one to find me! I can't imagine anyone else except myself to know Bernard. I must have heard my future self talking to me!#Kid_smile
(Was I so loud and fussy back then?)#Class:Purple
Ermm...I'm an engineer. #Kid_default
Agent X: What is enginger?
......You can understand engineer as a more practical scientist.#Kid_smile
Cool! I've always dreamt to be a scientist!
(This sounds like everyone's first dream since young, and 99% will give up, including yourself. You shrugged with a resigned smile)#Class:Purple
Agent X: Thanks for responding me! Now it's your turn. You can ask me anything - as a reward for rescuing Agent X from the evil dyrad!
*He's Bernard, not a evil dyrad or anything like that...
Agent X: Hoho, Bernard is so kind that he willingly accepted the character assignment of the evil dyrad! Let's give him a round of applause, for his fantastic performance!
    **Clap for Bernard
    ~KidAffinity++
    Agent X: Three cheers for Bernard!
        ***Three cheers for Bernard!
        Agent X: Encore!
        ~KidAffinity++
            ****Encore!
            ~KidAffinity++
            You are funny! Why you dumbly follow whatever I say?#Kid_smile
            *****Because I care for you.
            Agent X: That is disgustingly heart-warming. Alright, adult, thank you for that. #Kid_smile
            ~KidAffinity++
            ->AfterKidAffinity
            *****Coz I'm a dumbass. 
            Agent X: (Laugh out loud) That's so funny! You are nothing like the adults around me. I like you.
            ~KidAffinity=KidAffinity+2
            ->AfterKidAffinity
        ***Nope, that's all.
        Agent X: Alright. You are the adult. You have the say.#Kid_sad
        ->AfterKidAffinity
    **Errr, that's so embarrassing.
    Agent X: Alright. You are right. #Kid_sad
    ->AfterKidAffinity

*I know everything about you, little one, what should I ask?#Class:Blue
Agent X: I bet you never know at least one thing.I have not told anyone.#Teen_default
(You searched in your memory, trying to find anything that you felt to be important yet never shared with anyone.)#Class:Purple
Agent X: Do you know why Bernard is an evil dyrad?
(You really don't know that. You remember Bernard as a generic and remote concept, a symbolic item that signifies your childhood. Everyone might have such a symbolic memory - a doll, a pet, or a friend. You cannot remember the exact occurrences, but you treat them as a collection of reminiscences.)#Class:Purple 
What do you hide from me?#Class:Blue
Agent X: If I tell you, a secret counts no more. #Kid_smile
(A rare period of silence. Agent X shuts her mouth tight, as if keeping a confidential info that might threaten her supernatural institution. You know you will never know the story of Bernard. One day, you will forget the name Bernard. When that happens, you won’t even notice what else has disappeared with it )#Class:Purple

->AfterKidAffinity

=AfterKidAffinity
(It is already 11PM. You have to go back to work. You are left with two days to new year, and your progress is really worrying.)
*Kid, listen, I have to go back to work.
Agent X: Can't you just spend more time with me?#Kid_sad
I can't, I'm already spending longer than I supposed to be. 
Agent X: Would you answer my last question?

Agent X: What does it mean to grow up?
    Growing up means taking on responsibilities, and giving up on unrealistic ideals.
    Agent X: I'm going to school tomorrow.#Kid_default
    ***What does that mean?
    Agent X: Does that mean I will grow up?#Kid_sad
    **** Life is far longer than you think.
    Agent X: How long will it take to grow up?
    (You really don't know how to answer. It feels like your life has been stagnant for all this while. You are not sure if you are in your ultimate stage of growth, or you are stuck somewhere before that growth really happens.)#Class:Purple
    ->KidAffinityReach
    **** Yes, it's a beginning of your growth.
    (To you, growth is a process, growing up is a spectrum. Perhaps a beginning happens even earlier than her first day in school. It might have happened since the moment the question is asked.)#Class:Purple
    ->KidAffinityReach
    ***You are prepared, believe in yourself.
    (This is just a useless encouragement. A white lie. Only you know how tough it was for a village girl.)
    ->KidAffinityReach

    

=KidAffinityReach
Agent X: Will we meet again? Do you have anything else to say to me?#Teen_default
*Best of luck. I will go back to work.
->AfterWork
*{KidAffinity>3} I want to ask you a question.
    (You still remember that first day. In River Ville, parents do not have any awareness for pre-education. Basically you just go to school like a blank piece of paper. )#Class:Purple
    (There is a shuttle between River Ville and Rock Town. Every day you need to take that bus at 5:30AM, and reach school at 7:00AM for flag-raising ceremony.)#Class:Purple
    (This was a facility provided by the town to support the underdeveloped River Ville. Luckily you have Patty back then, life wasn't that hard.)
    (But you somehow find something missing deep in your heart. Like a missing jigsaw piece that does not really affect the full pattern.)#Class:Purple
    What is the first thing you want to do after you go to school and study things new?#Class:Blue
    (She remains silent with contemplation. After a while, she speaks with a sense of seriousness.)#Class:Purple#Teen_default
Agent X: I want to write out the stories between me and Bernard, as a record of our friendship, before he loses me or I loses him.#Teen_smile
Agent X: Oh, Bernard is calling me. Byebye!
(She left, as if she will see you tomorrow. )
(Yes, a story. She wanted to write a story. And that notebook was confiscated during class when she was doodling herself hiding in the tree hole."Be realistic!" Teacher said in a harsh tone.)#Class:Purple
(That was the last story she wrote, and Bernard was dead soon after she stopped writing. He was really an old tree with fragile structure, at the age of 30, you can tell it was natural. But Agent X couldn't. She thought it was her fault.)#Class:Purple
**It is all over. I need to move on.
->AfterWork
**Perhaps, as an adult, I can help her recollect that dream.
You made up your mind. Engineer might be part of your identity, but it fails to define you. A germ that was once inhibitted bud in a new winter after 24 years. Spring will come, and you will start something new in your life, or to reencounter your past.
~Dream=Dream+3
~DOS=DOS+2
->Day3



=AfterWork
(You worked for the whole evening until 2AM. Things go really smoothly. You believe you can complete everything by the beginning of new year.)#Class:Purple
It's been a long time since you last have a dream. You dream of someone calling your name but you never respond.#type_animation
Another dream almost come to your memory. That was you at the age of six, just before you go to primary school. You were so worried about the upcoming life.#type_animation
While excited about your chance to escape from here, you are also scared that one day you will be resigned to the fate as a village girl, get married early, continuing the life of your grandparents, your parents, and pass the same life to your daughter or son.#type_animation
You said so many "Hello" in that dream, yet only your own voice echos in darkness.#type_animation
You once believed that there is another self that can guide you through all difficulties. In your childhood years, you have been calling her for so many times: You arranged games for the two of you, hide in the tree hole, anticipating that one day you will be found.#type_animation
But no. Nothing happened. Your imaginary elder self, your friend, has never looked back to her past.#type_animation
It was that night you realized: You have to grow up from those useless imaginations. You are all alone.#type_animation
~Dream--
->Day3

===Day3===
It is the New Year's eve, two hours later, it will be a new day and a new year. #type_animation
You worked for the whole day for that final sprint, and just with one single click, the bug at train station will be fixed and the city will be fully lit up with the new year's bells.#type_animation
The system will record your operation - done before 12AM, it's counted as yours. Promotion, migration, fortune, achievement, everything you dreamt of, what you came to Light City for, will belong to you.#type_animation
This is how you persuade yourself. But you hesitated. You can't tell the reason. You have pushed that change to the system for thousands of times. It's just one step there. Why not press the button?#type_animation
Nevermind. Get yourself a drink and a snack first. Have some music - Press the button might be a good choice given the circumstances. You still have two hours left. #type_animation

->Menu3
=Menu3
->AfterDay3Selection

=AfterDay3Selection
??:Hey, we finally meet each other.
What's up?#Class:Blue
(Her figure renders on your screen. You see her tied hair, her badge, and her confident posture. This is how she shows herself. She is you.)#Class:Purple
Margaret.#Class:Blue
(Calling yourself with your own name gives a sense of strange intimacy. It seems that you own a name in every stage of your life: Agent X, Maggie, Professional Engineer Dr Grey, but you are seldomly addressed as Margaret.)#Class:Purple
(And it's a name that get along with you for the longest time in life.)#Class:Purple
Margaret:Towards the end of year, how are you feeling?#Adult_default
(She asks as if she knows nothing about you, or you indeed know minimal about yourself. )#Adult_default #Class:Purple
(Your history is like a Ship of Theseus, from that little River Ville to Light City, from your humble family to a teenager that comes to big city alone, from a wild girl to a professional engineer - Here and there, now and then.） #Adult_default#Class:Purple
(Time, space, identity...Everything is mixed. You are always positioning youself in vicissitudes, after all these reconstructions, are you still the self that you perceive to be?)#Adult_default#Class:Purple
That's a hard question to answer. Mixed feelings, perhaps. #Class:Blue
Margaret: Or a more detailed question, any New Year resolution or plans? After the full coverage of light, the next government initiative is to achieve the constant temperature zone - and follow-up activities such as pest control led by comfortable constant temperature.#Adult_default
Margaret: The Star City previously implemented genetic engineering to female mosquitos and effectively annihilated this species in the premise, such that the spread of dengue can be controlled. 
Margaret: I heard that this technology is in the midst of promotion in the country along with the constant temperature thingy, and Light City, the most developed city in the north, will be the first experimental region.#Adult_default
Which means that we will never have winter after then?
Margaret: Yes, but I thought you hate winter the most? You often complain about those bulky clothes and boots that make you feel like a seal crawling on the ground.#Adult_smile
*Oh ya, you are right. 
->EliminateWinter
*Complaints do not necessarily mean I hate winter. 
->ReserveWinter

=EliminateWinter
// TODO: Add narrative content here
-> END


=ReserveWinter
// TODO: Add narrative content here
-> END



-> END


