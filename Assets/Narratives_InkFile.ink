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
Before answering that, I should prompt you for an explanation of your existence first.
Errr, you are so boring and serious, and always view the world with an epistemological approach. #Teen_speechless
Are you speaking of yourself since you CLAIM to be my 15-year-old self, or I should call you Maggie?
Maggie: I'm nothing like you. But fair enough, that's how my friends call me. Do you remember Patty? Do you still have contact with her?#Teen_default
Kind of, if you call that regular New Year wishes broadcasted to everyone is a form of contact. 
Growing up is just so dull. What are you doing now?
I'm a light engineer in the Light City, making sure a region is fully covered by light sources.
Ever since we built the Dyson swarm, solar energy can be transmitted directly. Now every city is building full light coverage systems.
Maggie: For what? 
* Pragmatically feasible. Better for surveillance and security.
Maggie: Such a dry reasoning. Sounds like what I will write for my argumentative essay.#Teen_speechless
~Stability++
->Combine1
* A fully lit world is just a beautiful vision.
->DOL1

=DOL1
Maggie: Like achieving a straight A on your transcript? #Teen_default
*Totally.
~Achievement++
->Combine1
{Sadness==0:
*Not Exactly. More like my dream as always, to create a world with no darkness, conspiracies and evil. 
~Dream++
~DOL++
->Combine1
}
{Repulsion==0:
*Don't you think energy tech itself is fascinating? 
~Dream++
~DOR++
->Combine1
}

* I don't know. 
Maggie: OK, then we are on the same page: I don't know what I'm doing, and really cannot imagine at the age of 30 I will still be like this.#Teen_default
->IDK1

=IDK1
* The "Not knowing" state in your 15s and in my 30s is not the same.
One is not knowing what you want, the other is not knowing how to choose.
Maggie: If your life is a balance with two sides, your job is on one side, what will be your other side? 
{Repulsion==3 && Sadness==3: 
* Not sure, but I need to escape from my current life.
~Stability--
->Combine1
}

* Yeah, you're right. People never know the cutting point of growing up.
->Combine1

=Combine1
Maggie: Anyways, how are you? #Teen_default
It's the third time you are asking this question.#Class:Blue
Maggie: And NEVER get a proper answer. As a 15-year-old teenager I'm really curious about my future life.
I have told you, you want me to repeat? I'm a light engineer...#Class:Blue
Maggie: Urgggg not this one. I want to know about your LIFE. Come home alone, grab that instant noodle and a 10PM cup of coffee, play a sound track that almost explodes my brain. Is that your life?#Teen_speechless
Not everyday, there's an emergency. The area under me encounters a bug and I need to resolve it before the new year's deadline. #Teen_default
This will affect our year-end assessment as a department, and I'm unfortunately on the year-end call. #Class:Blue
My boss is really pushing hard. So I have to stay up to monitor and fix the real time issues. #Class:Blue
Maggie: Am I wasting your time then?
You know that and you are still asking. #Class:Blue
Maggie: Great to hear, I love wasting people's time, just like how I always waste Patty's precious studying hours. #Teen_smile
(Naughty girl...)#Class:Purple
Maggie: So what's your call tonight? #Teen_default
I fixed a few spots' coverage - great, my boss praised me for that. #Class:Blue
Left with one, a deserted train station at the Riverside district. #Class:Blue
The light network has definitely reached, but looks as if this specific place is resisting light from coming in.#Class:Blue
Moreover, detection system is completely lost there, so indeed my boss knows nothing about it.#Class:Blue
Maggie: A deserted train station... That's not how we call the Riverside Station. 
Maggie: Only this old station willingly accommodate those old-fashioned trains between Light City and my hometown.#Teen_sad
Now that line is also obsoleted. #Class:Blue
Maggie: That's really sad. I'm sorry for your loss.
I really didn't lose anything. Indeed, the train system has extinguished in the entire country.#Class:Blue 
Most of them are requisited to build city facilities, such as tairports and Helio Centers.#Class:Blue
Maggie: You are really talking like my chemistry teacher. #Teen_default
What?
Maggie: Speaking a language that I understand yet never really comprehend as if no explanation is needed.#Teen_speechless
* That's my intention. You don't need to know any of these terms in detail.#Class:Blue
Maggie: You should know better than anyone that I don't like to be treated as a kid.#Teen_anger
~TeenAffinity--
->Explanation
* I'm sorry, I should have explained to you.#Class:Blue
~TeenAffinity++
->Explanation

=Explanation
Anyways. Tairports, basically is the airtaxi ports.#Teen_default
No more taxis nowadays. You can just take autonomous flights wherever you go.#Class:Blue
And Helio Centres are like Tokamak reactors in the past, huge, dangerous, high tech, and supplies endless energy, but this time the energy is directly extracted from the sun.#Class:Blue
Maggie: OK, I'll buy that. At least you are more patient than the chem teacher.
Maggie: Luckily, I haven't grown up into a mean adult. #Teen_smile
* NO NO NO, I have the meanest manner behind the scene (make a harsh face)#Teen_default
~TeenAffinity++
->AfterTease
Maggie:(Laugh out loud) Woah looks like I will be scared! #Teen_smile
* Whatever.
->AfterTease

=AfterTease
Maggie: What are the consequences? I mean, if you fail to report.#Teen_default
If you really think about it - ya, nothing is really serious. Worst case scenario, losing my job. #Class:Blue
But most likely, no one will notice.#Class:Blue
Detection is missing. We move on. The city moves on.#Class:Blue
Maggie: Sounds nothing like an emergency.
Rationally yes. This detection issue is quite common around the system, and no effective solution for now. #Class:Blue
Even if someone finds it, I will just be one of the many cases, since I've already paid my dues elsewhere.#Class:Blue
BUT, if I report and fix it before New Year, I definitely will secure a performance-based early promotion.#Class:Blue
Maggie: That's attractive. Apart from higher pay, what will you get?
Longer holidays, fully remote working - plus an exchange program in Star City with all costs covered by my company. I've been craving for their chicken rice for years. #Class:Blue
{Repulsion==3:
Maggie: But you sound reluctant.
*Am I? I don't think so.#Class:Blue
Maggie: OK, then just assume I said nothing.
->Benefits
*......(Silence)#Class:Blue
Maybe the benefits need me more than I need them.
~Stability--
~Achievement--
->Benefits
}
{Repulsion==0&&Sadness==0:
Maggie: You sound excited. Does the vision of success excite you that much?#Teen_smile
* Yes! I'm super happy when thinking about what I can do with the higher pay.#Class:Blue
I may rent a single person apartment in Star City, facing the river and see the starry city on the other side. That's exactly the life I'm looking for. That's my goal of this everlasting migration - to find a home of my own.#Class:Blue
~Achievement++
->Benefits
* Of course! Star City has the most advanced technology. #Class:Blue
Apart from their light technology, I'd love to see their space tourism systems.#Class:Blue
Because they have much trading transportation for those crops planted on other planets, they have in the meantime developed the mature space transit that only needs a few thousand bucks.#Class:Blue
~Dream++
~DOL++
->Benefits
* Hmmm, indeed, just the vision of travelling to a new place excites me.#Class:Blue
Maggie: But you can travel whenever you want!
Looking at my duty. How can I leave all this behind?#Class:Blue
Maggie: You can resign. I'm serious.#Teen_default
And you will pay for my rents? Also my costs of travelling around?#Class:Blue
Maggie: No way! I'm still a little girl counting every penny just to have some savings.#Teen_smile
What are you saving for?#Class:Blue
Maggie: A trip to Star City. Patty has promised me, if she manages to get an offer from Light City University, her parents will reward her some money. Plus her private savings, that may be enough for a trip to Star City.
I have impression of that.#Class:Blue
Maggie: What's the outcome?
    {TeenAffinity>0:
    (She was asking in an eager manner. I really don't want to disappoint her.)#Class:Purple
    I prefer not to tell.#Class:Blue
    Maggie: OK. But anyways, you are going to Star City.
    - else:
    We didn't make it.#Class:Blue
    Maggie: Hmmm. Expected... #Teen_sad
    Maggie: I've thought of that, really. I didn't really think it will happen all this time.
    Maggie: I feel like I'm just saving money for that tiny promise of escape,
    Maggie: so that I'm still holding on to something solid in this strange city that never quite feels like mine.
    Maggie: But now, I think there's no longer a need for saving.
    ~DOT--
    ~Dream--
    }
->Benefits
}
->Benefits

=Benefits
Maggie: Another exchange program... We are always exchanging, physically and culturally.#Teen_sad
I can never forget the day you received a call about your successful application for a sponsored exchange to Light City.#Class:Blue
They called in multiple times when you were watching a movie with Patty. You guys took out all pocket money to buy yourselves one reckless day., and those phone calls came in such a frustrating way during the movie.#Class:Blue
You pushed that hang-up button hard to punish it for interrupting the movie. It went silent. After that two hours, you received the call again, answered "Yes" to the offer, and burst into tears in Patty's embrace.#Class:Blue
Maggie: From my perspective, that was the most courageous moment in my life. How do you view it after 15 years?#Teen_smile
(Chuckling) Courageous, what a word. Bold choice, I would say. You only thought you knew what you were stepping into.#Class:Blue
Maggie: Light City has a very different rhythm from River Ville. Back home, you were outstanding. In Light City, you were simply… one of many. And I truly miss Patty.#Teen_sad
{Sadness==3:
(Nostalgia settles quietly, like household lights slowly claiming the dark. You have not thought about Patty in a long time, but at that moment, you see her pimpled face and that typical smile with braces. )#Class:Purple
(Her dark eyes meet yours. You recognize that loneliness.#Class:Purple
(It is the same one that used to stare back at you in the boarding school hostel.)#Class:Purple
I almost forgot her face and what we have been through. I've grown so used to being alone.#Class:Blue
Maggie:You should go back and contact her some day. Now I cannot really call her that much, she's studying at a high school known for its strict, militarized discipline, so she really has limited access to her phone.
Maggie: Whenever the weekend comes, the first thing she does is to get back her phone and reply my random messages, I reply immediately, just to spend that precious hour of freedom with her.#Teen_smile
Maggie: "I'm striving to get to the same college as you. By then, I will have my phone with me, and have you every day!" That's what Patty said. Now you have your phone, why not give her a call?
* It feels so embarassing to contact a person that you have not talked to for a long time.#Class:Blue
Maggie: Ah, that's fair. Perhaps I should cherish that time before our friendship fades away.
->AfterFriend
* I'll try that, after this year-end catastrophe is over.#Class:Blue
Maggie: I bet you will.#Teen-smile
~Friend++
->AfterFriend
}
->AfterFriend

=AfterFriend
But look at me. I got it through. You've made it. I have settled down in this city. This will also be your future.#Class:Blue
Maggie: This really doesn't sound promising, if growing up and settling down means overworking until 11PM. #Teen_speechless
Thinking on the bright side: this IS your current life. We are used to this routine.#Class:Blue
Maggie: But I worked so hard, not for you to continue that routine! If I know I'm working for your hopeless future, I just don't give a shit - grades, competitions, rankings...#Teen_angry
Maggie: Those nights I cried for a failure to enter Olympiad finals. I put in all my efforts for the study hours. What's the meaning of all these trifles?
30 is not end of the world. There's still more unknowns ahead.#Class:Blue 
Maggie: What does turning to 30s mean? It doesn't really mean anything, unless you give meaning to it. 
Speaking of that, what do you think of your 30s if you never meet me?#Class:Blue 
Maggie: I was dreaming of a satisfying life, and scared of finding out that things never turn out as what I expect. 
This statement still holds today when I think about my 40s. We are always getting a "fear" advance to prevent ourselves from falling behind, just as if any cash advance is going to solve our current problems.#Class:Blue
We keep borrowing fear from the future, thinking it will protect us.But one day you realize, you're already living in the future you once tried to guard against.#Class:Blue
(Suddenly, the screen in front of you turns dark. Your work might have gone unsaved. Damn it - You don't really understand why in an age of constant energy supply, there is still no effective way to resolve computer blackout)#Class:Purple
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
(Her face gradually appears on your screen, from pixelated points to a clear figure. )#Class:Purple
(Like rendering a scene when you reach a new place in game. Strange.)#Class:Purple
Maggie: Power cut on my side, but tomorrow is my chem finals. So frustrating.#Teen_speechless
What should I do? Or what did you do?#Teen_default
(What did you do? that question comes to your mind. You definitely remember that night of power cut. That was a major incident in the Light City, and has been on the top1 popularity in social media discussion tags for consecutive weeks.)#Class:Purple 
(Everything was shut down suddenly. The city experienced drastic turmoil in the night.Bank systems collapsed, lots of embezzling attempts, and multiple car accidents occurred as the cars energy was also connected to the city's centralized supply.)#Class:Purple
(Critiques to the system surge. Key opinion leaders pinpoints the complacency of system: when energy becomes unlimited, storage techs are obsoleted with ignorance, as if there will never be any possibility of systematic collapse)#Class:Purple
(But you would never know about these that night. Or you won't care about it. What you care about, is only...)#Class：Purple
->Stars

=Stars
*[Your chemistry exam.]->Chemistry
#Class:Blue
*[The stars in the sky.]->StarrySky
#Class:Blue

=Chemistry
I revised my chem exam that evening for the last sprint, and passed with flying colors.#Class:Blue
Maggie: But how?#Teen_smile
Try to dig out your torch from the suitcase. I put everything I don't normally use in the suitcase. You should know, that suitcase is really occupying lots of space yet our hostel room is so tiny, so I used it as a storage.#Class:Blue
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
(That's why you are always working so hard, from teenage years till now. It is 28 Dec, 3 days to new year. You still have ample time to work.)
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
Are you staying at the rooftop study area?#Class:Blue
Maggie: Ya, you know me. I love that place, though it's too windy to keep my notes in control.
Look up.#Class:Blue
(Maggie's figure disappears from your screen. In a short while, you see stars emerging on your screen. You see the stars through Maggie's eyes. #Picture:StarryNight
//Or can we use Van Gogh's starry night as a CG here? Do not use the most famous one, but some other starry nights he drew?
(Everything including time has paused. You immerse in the darkness, feeling like home.)#Class:Purple 
(You cannot exactly remember the outcome of that chemistry test. And standing at this point of time, everything that once mattered doesn't matter to you anyway.)#Class:Purple
(What about your work? You try to imagine yourself at the age of 40. Will you regret for not fixing that error on time, which defers your promotion at every step; or you will regret for not looking up to the starry sky that night?)#Class:Purple
(Everything has returned to darkness and solitude. In that stagnant silence, you have a faint glimpse of your calendar: it is 28 Dec.)#Class:Purple 
(You still have three days to report to your boss, and that determines whether you are able to secure the earliest round of merit-based promotion next year.) #Class:Purple
(However, a sudden ideal struck you in your mind: is there anything more important, more compelling to decide apart from work?)#Class:Purple
(You have a vague sense of it, but you know you will still turn on your laptop shortly, work until 2AM, and get up at 7:30AM to catch the office shuttle tomorrow.)#Class:Purple
~Dream++
~DOT++
->Day2


===Day2===





I have been in the Light City for 15 years since my secondary school. #type_animation
I took a train here: upon alighting, I was struck by the darkness in this old railway station. #type_animation
Sitting on a bench beside the track, immersing myself in that silent piece of summer night,#type_animation
I waited until the next train comes.#type_animation
-> END

-> END


  -> END




 * There were two choices.
 * There were four lines of content.

- They lived happily ever after.
    -> END
