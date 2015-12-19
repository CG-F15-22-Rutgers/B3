using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using System;
using RootMotion.FinalIK;

public class Part3Tree : MonoBehaviour
{
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform clickbuttonPosition;
    public Transform peterstart;
    public Transform conversposition;

    public GameObject Peter;
    public GameObject Daniel;
    public GameObject converse1;
    public GameObject converse2;
    public GameObject sadperson;

    public GameObject ohterPeople1;
    public GameObject ohterPeople2;
    public GameObject ohterPeople3;



    public Vector3 Petertarget;
    public Vector3 Danieltarget;
    public Vector3 Converse1target;
    public Vector3 Converse2target;
    public Vector3 sadpersontarget;

    public FullBodyBipedEffector Effector;
    public InteractionObject Button;

    public bool PeterSelected = true;
    public bool DanielSelected = false;
    public bool sadpersonSelected = false;
    public bool converse1Selected = false;
    public bool converse2Selected = false;
    

    public bool Peter_know_Daniel_hide = false;
    public bool Peter_has_ball = false;
    public bool Peter_close_to_converse = false;
    public bool Peter_close_to_sad = false;
    public bool sadperson_refuse_tell_where_Daniel = false;
    public bool converse_refuse_tell_ask_th_sadman = false;

    public bool conversation_with_twoperson = false;
    public bool conversation_with_sadperson = false;
    public bool clicked_onpen_door = false;

    public bool conversation_with_twopersonfinished = false;
    public bool conversation_with_sadpersonfinished = false;
    public bool conversation_with_Daniel = false;
    public bool click_open_doorfinished = false;


    public bool cannotfindDaniel = false;
    public bool findDaniel = false;
    public bool storyend = false;

    public bool Daniel_Find_Peter = false;

    public float click_button_time;
    public bool click_button_flag = false;

    public float time = 0;

    //for test,set some test bool to test the IBT
    bool testIBTfirst = false;
    bool testIBTsecond = false;
    bool testConversation = true;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        Petertarget = peterstart.position;
        Danieltarget = Daniel.transform.position;
        Converse2target = converse2.transform.position;
        Converse1target = converse1.transform.position;
        sadpersontarget = sadperson.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        setStoryArc();
    }

    protected Node ST_ApproachAndWait(Transform target, GameObject participant)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node ST_Wander(GameObject participant)
    {
        return new DecoratorLoop(
            new Sequence(
                         this.ST_ApproachAndWait(wander1, participant),
                         this.ST_ApproachAndWait(wander2, participant),
                         this.ST_ApproachAndWait(wander3, participant)
             )
             );
    }

    //StoryTree
    protected Node ST_Story()
    {
        return new DecoratorLoop(
            new SequenceParallel(
                
                this.ST_ClickButtonArc(),
                //this.ST_ApproachAndWait(conversposition,Peter),
                this.ST_StoryStartConversation_with_2person_Arc(),
                this.ST_StoryConversation_with_sadperson_Arc(),
                this.ST_StoryConversation_with_Daniel_Arc(),
                this.ST_StoryDaniel_Find_Peter()
                //this.ST_StoryStartConversationArc(),
                //this.ST_TestAssert("first",testIBTfirst),
                //this.ST_TestAssert("second",testIBTsecond)
                )
            );
    }

    protected Node ST_StoryStartConversation_with_2person_Arc()
    {
        Val<bool> condition = Val.V(() => conversation_with_twoperson);
        Func<bool> con = () => (condition.Value == true);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                            converse1.GetComponent<BehaviorMecanim>().Node_OrientTowards(Peter.transform.position),
                                                            converse2.GetComponent<BehaviorMecanim>().Node_OrientTowards(Peter.transform.position),
                                                            this.ST_Converse_3_person(converse2,converse1, Peter,3),
                                                            new LeafTrace("conversation with 2person")
                                                            
                                                             ));
    }

    protected Node ST_StoryConversation_with_sadperson_Arc()
    {
        Val<bool> condition = Val.V(() => conversation_with_sadperson);
        Func<bool> con = () => (condition.Value == true);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                            sadperson.GetComponent<BehaviorMecanim>().Node_OrientTowards(Peter.transform.position),
                                                            new Sequence( this.ST_Converse_2_person(sadperson, Peter, 3),
                                                                          sadperson.GetComponent<BehaviorMecanim>().Node_OrientTowards(Daniel.transform.position),    
                                                                          this.ST_Gestures_Hand(sadperson, "pointing", 1000)
                                                                            ),
                                                            new LeafTrace("conversation with sadperson")
                                                             ));
    }

    protected Node ST_StoryConversation_with_Daniel_Arc()
    {
        Val<bool> condition = Val.V(() => conversation_with_Daniel);
        Func<bool> con = () => (condition.Value == true);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                            Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(Peter.transform.position),
                                                            this.ST_Converse_2_person(Daniel, Peter, 3),                                                                                                              
                                                            new LeafTrace("conversation with Daniel")
                                                             ));
    }

    protected Node ST_StoryDaniel_Find_Peter()
    {
        Val<bool> condition = Val.V(() => Daniel_Find_Peter);
        Func<bool> con = () => (condition.Value == true);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                            this.ST_ApproachAndWait(Peter.transform,Daniel),
                                                            new LeafTrace("Daniel find Peter")
                                                             ));
    }

    protected Node ST_Move_to_2person()
    {
        Func<bool> con = () => (PeterSelected == false);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                            this.ST_Move_to_Peter(Peter,conversposition.position)
                                                             ));
    }



    protected Node ST_ClickButtonArc()
    {
        Val<bool> condition = Val.V(() => clicked_onpen_door);
        Func<bool> con = () => (condition.Value == true);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             this.ST_ClickButton(Peter,Effector,Button),
                                                             new LeafTrace("clickbutton")                                     
                                                             ));
    }

    protected Node ST_Goto_Button()
    {
        return this.ST_ApproachAndWait(clickbuttonPosition, Peter);
    }

    //MonitorInput
    protected Node ST_ClickButton(GameObject clicker, FullBodyBipedEffector effector, InteractionObject button)
    {
        Val<FullBodyBipedEffector> E = Val.V(() => effector);
        Val<InteractionObject> B = Val.V(() => button);
        return new Sequence(new LeafTrace("Interaction"), clicker.GetComponent<BehaviorMecanim>().Node_StartInteraction(E, B));
    }

    //MonitorStoryState
    protected Node ST_MonitorStoryState()
    {
        return new DecoratorLoop(
            new SequenceParallel(
                //new LeafTrace("MonitorStoryState")
                )
            );
    }


    protected Node ST_IBT()
    {

        //Func<bool> con = () => (testIBT == false);
        //Node trigger = new DecoratorLoop(new LeafAssert(con));
        //start building IBT

        //now for the total Interactive Behavior Tree
        //Three trees:StoryTree MonitorInput MonitorStoryState

        return new SequenceParallel(
                this.ST_MonitorInput(),
                this.ST_MonitorStoryState(),
                this.ST_Story()
                );
    }

    protected Node ST_TestAssert(string log,bool condition)
    {

        Func<bool> con = () => (condition == false);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(trigger, new DecoratorLoop(new LeafTrace("2"))));
    }

    protected Node BuildTreeRoot()
    {
        return
          new DecoratorLoop(
                new SequenceParallel(
                    this.ST_IBT(),
                    this.ST_Wander(ohterPeople1),
                    this.ST_Wander(ohterPeople2),
                    this.ST_Wander(ohterPeople3),
                    new LeafWait(1000)
                    )
                )
                            ;
    }


    protected Node ST_MonitorInput()
    {
        return new DecoratorLoop(
            new SequenceParallel(
                //new LeafTrace("MonitorInput")
                this.ST_userInteractionMovePeter(),
                this.ST_userInteractionMoveConverse1(),
                this.ST_userInteractionMoveDaniel(),
                this.ST_userInteractionMovesadPerson(),
                this.ST_userInteractionMoveConverse2()
                )
            );
    }


    void setStoryArc()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("environment"))
                {
                    if (PeterSelected)
                    {
                        Petertarget = hit.point;
                    }
                    else if (DanielSelected)
                        Danieltarget = hit.point;
                    else if (sadpersonSelected)
                        sadpersontarget = hit.point;
                    else if (converse1Selected)
                        Converse1target = hit.point;
                    else if (converse2Selected)
                        Converse2target = hit.point;
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
            testIBTfirst = false;
        if (Input.GetKey(KeyCode.B))
            testIBTfirst = true;
        if (Input.GetKey(KeyCode.C))
            testIBTfirst = false;
        if (Input.GetKey(KeyCode.D))
            testIBTfirst = true;

        //if (!PeterSelected)
        //{
        //    if (Vector3.Distance(clickbuttonPosition.position, Peter.transform.position) < 4)
        //    {
        //        clicked_onpen_door = true;
        //    }
        //    else if (Vector3.Distance(sadperson.transform.position, Peter.transform.position) < 4)
        //    {
        //        conversation_with_sadperson = true;
        //    }
        //    else if (Vector3.Distance(Peter.transform.position, converse1.transform.position) < 4 && Vector3.Distance(Peter.transform.position, converse2.transform.position) < 4)
        //    {
        //        conversation_with_twoperson = true;
        //    }
        //}

        if(Time.time - time > 40)
        {
            Daniel_Find_Peter = true;
        }

        if (PeterSelected)
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (Vector3.Distance(clickbuttonPosition.position, Peter.transform.position) < 2)
                {
                    clicked_onpen_door = true;
                }
                else
                {
                    clicked_onpen_door = false;
                }
                 if (Vector3.Distance(sadperson.transform.position, Peter.transform.position) < 5)
                {
                    conversation_with_sadperson = true;
                }
                else
                {
                    conversation_with_sadperson = false;
                }
                if (Vector3.Distance(Peter.transform.position, converse1.transform.position) < 4 && Vector3.Distance(Peter.transform.position, converse2.transform.position) < 4)
                {
                    conversation_with_twoperson = true;
                }
                else
                {
                    conversation_with_twoperson = false;
                }

                if (Vector3.Distance(Daniel.transform.position, Peter.transform.position) < 5)
                {
                    conversation_with_Daniel = true;
                }
                else
                {
                    conversation_with_Daniel = false;
                }


                if (Vector3.Distance(Daniel.transform.position, Peter.transform.position) < 4)
                {
                    Daniel_Find_Peter = false;
                }
            }
        }


        if (converse1Selected)
        {
            if (Input.GetKey(KeyCode.X))
            {
                converse_refuse_tell_ask_th_sadman = true;
            }

        }
        if (converse2Selected)
        {
            if (Input.GetKey(KeyCode.X))
            {
                converse_refuse_tell_ask_th_sadman = true;
            }
        }
        if (sadpersonSelected)
        {
            if (Input.GetKey(KeyCode.X))
            {

            }
        }
    }


    protected Node ST_userInteractionMovePeter()
    {
        Val<bool> condition = Val.V(() => PeterSelected);
        Func<bool> con = () => (condition.Value);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             new DecoratorLoop(this.ST_Move_to_Peter(Peter, Petertarget)),
                                                             new LeafTrace("Moveto")
                                                             ));
    }


    protected Node ST_userInteractionMoveDaniel()
    {
        Val<bool> condition = Val.V(() => DanielSelected);
        Func<bool> con = () => (condition.Value);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             new DecoratorLoop(this.ST_Move_to_Daniel(Daniel, Danieltarget)),
                                                             new LeafTrace("Moveto")
                                                             ));
    }

    protected Node ST_userInteractionMoveConverse1()
    {
        Val<bool> condition = Val.V(() => converse1Selected);
        Func<bool> con = () => (condition.Value);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             new DecoratorLoop(this.ST_Move_to_Converse1(converse1, Converse1target)),
                                                             new LeafTrace("Moveto")
                                                             ));
    }

    protected Node ST_userInteractionMoveConverse2()
    {
        Val<bool> condition = Val.V(() => converse2Selected);
        Func<bool> con = () => (condition.Value);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             new DecoratorLoop(this.ST_Move_to_Converse2(converse2, Converse2target)),
                                                             new LeafTrace("Moveto")
                                                             ));
    }

    protected Node ST_userInteractionMovesadPerson()
    {
        Val<bool> condition = Val.V(() => sadpersonSelected);
        Func<bool> con = () => (condition.Value);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success,
                                        new SequenceParallel(trigger,
                                                             new DecoratorLoop(this.ST_Move_to_sadPerson(sadperson, sadpersontarget)),
                                                             new LeafTrace("Moveto")
                                                             ));

    }

    protected Node ST_Move_to_Peter(GameObject person, Vector3 target1)
    {
        //Debug.Log(target);
        Val<Vector3> position = Val.V(() => Petertarget);
        return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position));
    }

    protected Node ST_Move_to_Daniel(GameObject person, Vector3 target1)
    {
        //Debug.Log(target);
        Val<Vector3> position = Val.V(() => Danieltarget);
        return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position));
    }

    protected Node ST_Move_to_Converse1(GameObject person, Vector3 target1)
    {
        //Debug.Log(target);
        Val<Vector3> position = Val.V(() => Converse1target);
        return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position));
    }

    protected Node ST_Move_to_Converse2(GameObject person, Vector3 target1)
    {
        //Debug.Log(target);
        Val<Vector3> position = Val.V(() => Converse2target);
        return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position));
    }

    protected Node ST_Move_to_sadPerson(GameObject person, Vector3 target1)
    {
        //Debug.Log(target);
        Val<Vector3> position = Val.V(() => sadpersontarget);
        return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position));
    }



    //animation
    protected Node ST_Gestures_Body(GameObject P, string name, long time)
    {
        Val<string> Gesture_name = Val.V(() => name);
        Val<bool> start = Val.V(() => true);
        return new Sequence(P.GetComponent<BehaviorMecanim>().Node_BodyAnimation(Gesture_name, start),
                            new LeafWait(time),
                            P.GetComponent<BehaviorMecanim>().Node_BodyAnimation(Gesture_name, false));
    }
    protected Node ST_Gestures_Face(GameObject P, string name, long time)
    {
        Val<string> Gesture_name = Val.V(() => name);
        Val<bool> start = Val.V(() => true);
        return new Sequence(P.GetComponent<BehaviorMecanim>().Node_FaceAnimation(Gesture_name, start),
                            new LeafWait(time),
                            P.GetComponent<BehaviorMecanim>().Node_FaceAnimation(Gesture_name, false));
    }
    protected Node ST_Gestures_Hand(GameObject P, string name, long time)
    {
        Val<string> Gesture_name = Val.V(() => name);
        Val<bool> start = Val.V(() => true);
        return new Sequence(P.GetComponent<BehaviorMecanim>().Node_HandAnimation(Gesture_name, start),
                            new LeafWait(time),
                            P.GetComponent<BehaviorMecanim>().Node_HandAnimation(Gesture_name, false));
    }

    protected Node ST_Converse_3_person(GameObject person1, GameObject person2, GameObject person3, int loop)
    {

        return new DecoratorLoop(loop,
         new Sequence(
                  new SelectorRandom(
                  //this.ST_Gestures_Body(person1, "throw", 1000),
                  new LeafWait(200),
                  this.ST_Gestures_Hand(person1, "beingcocky", 1000),
                  this.ST_Gestures_Hand(person1, "cheer", 1000)
                  ),
                   new SelectorRandom(
                       new LeafWait(100),
                  //this.ST_Gestures_Body(person2, "throw", 1000),
                  this.ST_Gestures_Hand(person2, "beingcocky", 1000),
                  this.ST_Gestures_Hand(person2, "cheer", 1000)
                  ),
                    new SelectorRandom(
                        new LeafWait(300),
                  //this.ST_Gestures_Body(person3, "throw", 1000),
                  this.ST_Gestures_Hand(person3, "beingcocky", 1000),
                  this.ST_Gestures_Hand(person3, "cheer", 1000)
                  )
                  )

          );
    }

    protected Node ST_Converse_2_person(GameObject person1,GameObject person2,int loop)
    {

        return new DecoratorLoop(loop,
         new Sequence(
                  new SelectorRandom(
                  //this.ST_Gestures_Body(person1, "throw", 1000),
                  new LeafWait(200),
                  this.ST_Gestures_Hand(person1, "beingcocky", 1000),
                  this.ST_Gestures_Hand(person1, "cheer", 1000)
                  ),
                   new SelectorRandom(
                       new LeafWait(100),
                  //this.ST_Gestures_Body(person2, "throw", 1000),
                  this.ST_Gestures_Hand(person2, "beingcocky", 1000),
                  this.ST_Gestures_Hand(person2, "cheer", 1000)
                  )
                  )

          );
    }

    public void selectPeter()
    {
        PeterSelected = true;
        DanielSelected = false;
        sadpersonSelected = false;
        converse1Selected = false;
        converse2Selected = false;
    }

    public void selectDaniel()
    {
        PeterSelected = false;
        DanielSelected = true;
        sadpersonSelected = false;
        converse1Selected = false;
        converse2Selected = false;

    }

    public void selectConverse1()
    {
        PeterSelected = false;
        DanielSelected = false;
        sadpersonSelected = false;
        converse1Selected = true;
        converse2Selected = false;
    }

    public void selectConverse2()
    {
        PeterSelected = false;
        DanielSelected = false;
        sadpersonSelected = false;
        converse1Selected = false;
        converse2Selected = true;

    }

    public void sadPerson()
    {
        PeterSelected = false;
        DanielSelected = false;
        sadpersonSelected = true;
        converse1Selected = false;
        converse2Selected = false;
    }
}
