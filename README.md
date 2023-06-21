<h1 align="center">Spread</h1>

<h3 align="center">What is it?</h3>
  <img src="https://github.com/Kosciach/Spread/assets/97178996/54a33a24-7704-496b-9574-55255b869a56" alt="SpreadScreenshot">
  <p align="center">
    Spread is a fullbody, zombie, survival game in early development.
    Big game being made by single person might sound like a really bad game, but previous experiences taught me to take thing slow and focus on details.
    Spread and its 2 previos version are a big lesson for me, letting me expand my skills with practical approach and gain experience.
  </p>

<br>
<h3 align="center">What was wrong with previous versions?</h3>
  <p align="center">
    I didn't care about quality while creating previous versions. I wanted to make them fast, this was the very thing that caused these projects to fail.
    3d models were looking really bad, code was terrible, animations were not working together, there was a lot of bugs.
The most important thing is that they were made with almost no experience.
Good thing is that I realized my mistakes and learned from them, looking at these 2 projects as studying material.
  </p>

<br>
<h3 align="center">What I do diffrently now?</h3>
  <p align="center">
    Now I am taking things slow, not only making the game work but adding detail, little things that make a lot of diffrence.
I know what my goal is and I know I can't take any shortcuts, it is to make really good game, that I can show people with proud.
My work is organized, everyday I prepare list of task for the day, after I finish a send them to github.
    Most of the time as one commit, except the times I do something bigger, this is one of the things I things I need to change, do more commits.
After I finish writing a script I thing how to improve it, if I learn something new I implement it to my code.
  </p>

<br>

---

<br>

<h2 align="center">Mechanics</h2>

<br>
<h3 align="center">Fullbody</h3>
<h2 align="center"> </h2>
<p align="center">
  Spread uses fullbody character without seperate hands or legs, however hands are rendered by seperate "handsCamera".
  Character is animated using unity animator and animations rigging.
</p>
<h4 align="center">--Animator--</h4>
<p align="center">
  Animator has 5 layers, Base, CombatBase, CombatAnim, TopBodyStabilizer, Crouch which are all controller from PlayerAnimatorController.<br>
  Base - Main layer, containing states responsible for ladder, climbing, inAir(jumping, falling, landing), Swim...<br>
  CombatBase - overrides top body with T-Pose animation.<br>
  CombatAnim - hold all baked combat animations, like reload and bolt action charge.<br>
  TopBodyStabilizer - overrides layers in order to make top parts of the body more stable.<br>
  Crouch - crouch had to be put on seperate layer, to achieve correct player animations.
</p>
<h4 align="center">--Ik--</h4>
<p align="center">
  Controller by a lot of controllers, like iklayers, weapon, fingers...<br>
  Ik is only and addition for most of baked animations, but for combat it is opposite.
  Weapons and hands in combat mode are animated procedulary with mainly WeaponAnimator, which doesn't do much on its own, because it holds references to a lot of smaller scripts, mainPositioner, sway, bobbing...
</p>



<br>
<h3 align="center">Cameras</h3>
<h2 align="center"> </h2>
<p align="center">
  There are two camera:<br>
  Main - for everything except hands.<br>
  Hands - just for hands.
</p>
<p align="center">
  Only main camera uses cinemachine, but both of them have their own controller, seperated into moving, rotating.
  Each camera has additionall smaller controllers like, toggle or fov.<br>
  Most of the time handsCamera will be enabled, but in states like ladder or swim, hands camera is toggled off to make main camera render hands.<br>
  Both camera are moved and rotated depending on player states to get as good feeling as possible.<br>
  Some ik layers and baked animations affect cameras transforms slightly, that happens because both hads and main camera are placed in players head bone.
</p>




<br>
<h3 align="center">Movement</h3>
<h2 align="center"> </h2>
<p></p>

<br>
<h3 align="center">Combat</h3>
<h2 align="center"> </h2>
<p></p>
