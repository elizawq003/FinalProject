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

// External Function
EXTERNAL EasterEggTrigger(ID)

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
It's the third time you are asking this question.
Maggie: And NEVER get a proper answer. As a 15-year-old teenager I'm really curious about my future life.
I have told you, you want me to repeat? I'm a light engineer...
Maggie: Urgggg not this one. I want to know about your LIFE. Come home alone, grab that instant noodle and a 10PM cup of coffee, play a sound track that almost explodes my brain. Is that your life?#Teen_speechless
Not everyday, there's an emergency. The area under me encounters a bug and I need to resolve it before the new year's deadline. #Teen_default
This will affect our year-end assessment as a department, and I'm unfortunately on the year-end call. 
My boss is really pushing hard. So I have to stay up to monitor and fix the real time issues. 
Maggie: Am I wasting your time then?
You know that and you are still asking. 
Maggie: Great to hear, I love wasting people's time, just like how I always waste Patty's precious studying hours. #Teen_smile
(Naughty girl...)
Maggie: So what's your call tonight? #Teen_default
I fixed a few spots' coverage - great, my boss praised me for that. 
Left with one, a deserted train station at the Riverside district. 
The light network has definitely reached, but looks as if this specific place is resisting light from coming in.
Moreover, detection system is completely lost there, so indeed my boss knows nothing about it.
Maggie: A deserted train station... That's not how we call the Riverside Station. 
Maggie: Only this old station willingly accommodate those old-fashioned trains between the Light City and my hometown.#Teen_sad
Now that line is also obsoleted. 
Maggie: That's really sad. I'm sorry for your loss.
I really didn't lose anything.
Indeed, the train system has extinguished in the entire country. 
Most of them are requisited to build city facilities, such as tairports and Helio Centers.
Maggie: You are really talking like my chemistry teacher. #Teen_default
What?
Maggie: Speaking a language that I understand yet never really comprehend as if no explanation is needed.#Teen_speechless
OK my bad. Tairports, basically is the airtaxi ports.#Teen_default
No more taxis nowadays. You can just take autonomous flights wherever you go.
And Helio Centres are like Tokamak reactors in the past, huge, dangerous, high tech, and supplies endless energy,
but this time the energy is directly extracted from the sun.
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
If you really think about it - ya, nothing is really serious. Worst case scenario, losing my job. 
But most likely, no one will notice.
Detection is missing. We move on. The city moves on.
Maggie: Sounds nothing like an emergency.
Rationally yes. This detection issue is quite common around the system, and no effective solution for now. 
Even if someone finds it, I will just be one of the many cases, since I've already paid my dues elsewhere.
BUT, if I report and fix it before New Year, I definitely will secure a performance-based early promotion.
Maggie: That's attractive. Apart from higher pay, what will you get?
Longer holidays, fully remote working - plus an exchange program in Star City with all costs covered by my company. I've been craving for their chicken rice for years. 
{Repulsion==3:
Maggie: But you sound reluctant.
*Am I? I don't think so.
Maggie: OK, then just assume I said nothing.
->Benefits
*......(Silence)
Maybe the benefits need me more than I need them.
~Stability--
~Achievement--
->Benefits
}

->Benefits

=Benefits
Maggie: Another exchange program... We are always exchanging, physically and culturally.#Teen_sad
I can never forget the day you received a call about your successful application for a sponsored exchange to Light City.
They called in multiple times when you were watching a movie with Patty. You guys took out all pocket money to buy yourselves one reckless day., and those phone calls came in such a frustrating way during the movie.
You pushed that hang-up button hard to punish it for interrupting the movie. It went silent. After that two hours, you received the call again, answered "Yes" to the offer, and burst into tears in Patty's embrace.
Maggie: From my perspective, that was the most courageous moment in my life. How do you view it after 15 years?#Teen_smile
(Chuckling) Courageous, what a word. Bold choice, I would say. You only thought you knew what you were stepping into.
Maggie: Light City has a very different rhythm from River Ville. Back home, you were outstanding. In Light City, you were simply… one of many. And I truly miss Patty.#Teen_sad
{Sadness==3:
(Nostalgia settles quietly, like household lights slowly claiming the dark. You have not thought about Patty in a long time, but at that moment, you see her pimpled face and that typical smile with braces. 
Her dark eyes meet yours.You recognize that loneliness.
It is the same one that used to stare back at you in the boarding school hostel.)
I almost forgot her face and what we have been through. I've grown so used to being alone.
Maggie:You should go back and contact her some day. Now I cannot really call her that much, she's studying at a high school known for its strict, militarized discipline, so she really has limited access to her phone.
Maggie: Whenever the weekend comes, the first thing she does is to get back her phone and reply my random messages, I reply immediately, just to spend that precious hour of freedom with her.#Teen_smile
Maggie: "I'm striving to get to the same college as you. By then, I will have my phone with me, and have you every day!" That's what Patty said. Now you have your phone, why not give her a call?
* It feels so embarassing to contact a person that you have not talked to for a long time.
Maggie: Ah, that's fair. Perhaps I should cherish that time before our friendship fades away.
->AfterFriend
* I'll try that, after this year-end catastrophe is over.
Maggie: I'm sure you will.
~Friend++
->AfterFriend
}
->AfterFriend

=AfterFriend








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
