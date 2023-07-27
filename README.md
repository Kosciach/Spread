<h1 align="center">Spread</h1>
<h3 align="center">Readme work in progress!(Image is up to date)</h3>

<h3 align="center">Currently working on: Throwables animations.</h3>
  <img src="https://github.com/Kosciach/Spread/assets/97178996/fc92a88d-e0a2-436c-8141-159c41e477ac" alt="SpreadScreenshot">
  <p align="center">
    Spread is my main project, which is going to be a first person story game.
    Heavily inspired by other popular games like, Dying Light, Far cry series, Tlou.
    Since I am the only one creating this game, I like to take my time and polish the new features and make the game feel good and not cheap or crappy.
    Opposite to what I did with the previous 2 versions.
  </p>

<br>
<h3 align="center">2 Previous versions</h3>
  <p align="center">
    These versions projects were supposed to be the "dream game", made fast, just make things somewhat work and skip to other things.<br>
There was no planning and both these games felt terrible, and both were abbandoned.<br>
But as always, I have learned from my mistakes and these projects taught me a lot.
  </p>

<br>
<h3 align="center">What I do diffrently now?</h3>
  <p align="center">
    This time I decided to take a diffrent approach and make a good game even if it took a looong time.<br>
Things are planned, and thought out, there is no taking a shortway.<br>
    To add new features, current state of the game must be tested and all bugs fixed.<br>
    If some aspect of a game doesnt fell right, I put it on a list of things to change/fix and take care of it as soon as I am done with current feature.
  </p>

<br>

---

<br>

<h2 align="center">Mechanics</h2>

<br>
<h3 align="center">Fullbody</h3>
<h2 align="center"> </h2>
<p align="center">
  Spread uses fullbody character without seperate hands or legs, however hands are rendered by seperate "handsCamera".<br>
  Character is animated using unity animator and animations rigging.
</p>
<h4 align="center">--Animator--</h4>
<p align="center">
  Animator has few layers like  Base, CombatBase, CombatAnim, TopBodyStabilizer, Crouch which are all controller from PlayerAnimatorController.<br>
</p>
<h4 align="center">--Ik--</h4>
<p align="center">
  Ik is used to bend player according to where he is looking, but the main use is weapons animatins.<br>
Making baked animations is not my thing, but weapons have to be animated, so I use procedular animations.<br>
  Each weapon has its own data(scriptable object), which hold info damage, range etc, and diffrent vectors.<br>
These vectors are packed in pairs(PosRot) and represent the way player holds the weapons, and are used to move ik target.<br>
If player chooses to aim weapon, one of WeaponAnimators sub controllers(MainTransformer), lerps targets to aim vectors of equiped weapon.<br>
To also apply thing like bobbing, sway, jump, crouch WeaponAnimators uses vectors from sub controllers, combining and applying them to targets.<br>
Right hand Ik target is seperated into base and extra game objects (base for positioning weapon, extra for sway, bob etc).
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
  Only main camera uses cinemachine, but both of them have their own controller, seperated into moving, rotating.<br>
  Each camera has additionall smaller controllers like, toggle or fov.<br>
  Most of the time handsCamera will be enabled, but in states like ladder or swim, hands camera is toggled off to make main camera render hands.<br>
  Both camera are moved and rotated depending on player states to get as good feeling as possible.<br>
  Some ik layers and baked animations affect cameras transforms slightly, that happens because both hads and main camera are placed in players head bone.<br>
  Of course this is intended, like running animations makes the camera bob slightly, replicating what we see while running.
</p>



<br>
<h3 align="center">PlayerStateMachine</h3>
<h2 align="center"> </h2>
<p>
  The main script of player, serves as boss, holds references to every player script, if anythins wants to access other players scripts, it must have reference to this StateMachine.<br>
  Controllers are seperated into groups and added into structs to organize the work, combat to combat, animations to animations...<br><br>
  SwitchController is the one who manages states changing, or more precisely, contains a method for each state, which changes StateMachine switch, which is used by states to decide what states should happen in the current frame. SwitchController also decides if switch can be changed, without this switch could be changed but state not.
</p>


<br>
<h3 align="center">Movement</h3>
<h2 align="center"> </h2>
<p>
  Movement is like any other big controller, separeted into a one main and few subcontrollers, in this case, OnGround, Swim Ladder etc.<br>
Method from subcontrollers are called from PlayerStates(Onground from states like idle, walk, run, crouch, swim from swim and underwater...)<br>
  To move the player I use character controller, but this doesnt have gravity like rigidbody so VerticalVelocity takes care of it.<br>
</p>

<br>
<h3 align="center">VerticalVelocity</h3>
<h2 align="center"> </h2>
<p>
  Another big controller, it purpose is to check ground, slopes, apply gravity, jumps.<br>
  Most methods are like in movement called from states.
</p>

<br>
<h3 align="center">Combat</h3>
<h2 align="center"> </h2>
<p></p>
