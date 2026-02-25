//Emotion Var
//0=calm, 1=medium,2=anxious for example
VAR Anxiety=0
VAR Sadness=0
VAR Repulsion=0

// Ending Var
VAR Dream=0
VAR Achievement=0
VAR Stability=0

// Dream Index:Dream of light (DOL), Dream of research (DOR),Dream of travel (DOT), Dream of story (DOS)
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
Growing up is just so dull. What are you doing now?#Teen_default
I'm a light engineer in the Light City, making sure a region is fully covered by light sources.
Ever since we built the Dyson swarm, solar energy can be transmitted directly. Now every city is building full light coverage systems.
Maggie: For what? #Teen_default
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
Maggie: If your life is a balance with two sides, your job is on one side, what will be your other side? #Teen_default
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
Not everyday, there's an emergency. The area under me encounters a bug and I need to resolve it before the new year's deadline. This will affect our year-end assessment as a department, and I'm unfortunately on the year-end call. My boss is really pushing hard. So I have to stay up to monitor and fix the real time issues. 
Maggie: Am I wasting your time then?#Teen_default
You know that and you are still asking. 
Maggie: Great to hear, I love wasting people's time, just like how I always waste Patty's precious studying hours. #Teen_smile
(Naughty girl...)
Maggie: So what's your call tonight? #Teen_default
I fixed a few spots' coverage but left with one, a deserted train station at the Riverside district. The light network has definitely reached, but looks as if this specific place is resisting light from coming in.#Teen_default
Maggie: A deserted train station... That's not how we call the Riverside Station. #Teen_default
Maggie: Only this old station willingly accommodate those old-fashioned trains between the Light City and my hometown.#Teen_default
Now that line is also obsoleted. #Teen_default
Maggie: That's really sad. I'm sorry for your loss.#Teen_sad
I really didn't lose anything.
Indeed, the train system has extinguished in the entire country. 
Most of them are requisited to build city facilities, such as tairports and Helio Centers.
Maggie: You are really talking like my chemistry teacher. #Teen-default
What?
Maggie: Speaking a language that I understand yet never really comprehend as if no explanation is needed.#Teen_default
OK my bad. Tairports, basically is the airtaxi ports. 
No more taxis nowadays. You can just take autonomous flights wherever you go.
And Helio Centres are like Tokamak reactors in the past, huge, dangerous, high tech, and supplies endless energy,
but this time the energy is directly extracted from the sun.
Maggie: OK, I'll buy that. At least you are more patient than the chem teacher.#Teen_default
Maggie: Luckily, I haven't grown up into a mean adult. #Teen_smile
* NO NO NO, I have the meanest manner behind the scene (make a harsh face)
~TeenAffinity++
->AfterTease
Maggie:(Laugh out loud) Woah looks like I will be scared! #Teen_smile
* Whatever.
->AfterTease

=AfterTease
Maggie:I want to know more 







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
