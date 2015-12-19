using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree : MonoBehaviour
{
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;

    public GameObject ohterPeople1;
    public GameObject ohterPeople2;
    public GameObject ohterPeople3;




    public Transform sadPosition, ballPosition, ThrowPosition, nearHidePosition, waterPosition;
    public Transform conversePosition;
    public Transform HidePosition;
    public Transform ButtonPosition;
    public Transform make_choice_position;
    public GameObject Daniel;
    public GameObject Peter;
    public GameObject ball_to_throw;
    public GameObject test;
    public GameObject throwDirection;
    public bool arrive_at_choice = false;

    public GameObject converse_1;
    public GameObject converse_2;
    //public GameObject shakeObject;
    
    public GameObject sadperson;


    //public GameObject Button;
    public InteractionObject Button;
    //interaction
    public InteractionObject Daniel_righthand;
    public InteractionObject sad_righthand;
    public InteractionObject Ball;
    public InteractionObject shakeObject_D;
    public InteractionObject shakeObject_S;
    public FullBodyBipedEffector Effector;
    public FullBodyBipedEffector shakehand;

    


    //throw ball 


    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(Daniel.transform.position, make_choice_position.position) < 0.5)
        //{
        //    arrive_at_choice = true;
        //}


    }

    protected Node ST_ApproachAndWait(GameObject player, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(new LeafTrace("Goingto" + target.position), player.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }
    protected Node ST_PickUpBall(GameObject picker, FullBodyBipedEffector effector, InteractionObject ball)
    {
        Val<FullBodyBipedEffector> E = Val.V(() => effector);
        Val<InteractionObject> B = Val.V(() => ball);
        //ball_to_throw.GetComponent<Rigidbody>().useGravity = false;
        //ball_to_throw.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        return new Sequence(new LeafTrace("Interaction"), picker.GetComponent<BehaviorMecanim>().Node_StartInteraction(E, B), new LeafWait(1000));
    }
    protected Node ST_shakehand(GameObject sad,GameObject D, FullBodyBipedEffector effector, InteractionObject D_r, InteractionObject sad_r)
    {
        Val<FullBodyBipedEffector> E = Val.V(() => effector);
        Val<InteractionObject> DD = Val.V(() => D_r);
        Val<InteractionObject> RR = Val.V(() => sad_r);
        //ball_to_throw.GetComponent<Rigidbody>().useGravity = false;
        //ball_to_throw.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        return new SequenceParallel(
            new Sequence( new LeafTrace("Interaction"),D.GetComponent<BehaviorMecanim>().Node_StartInteraction(E, RR), new LeafWait(1000)),
            new Sequence(new LeafTrace("Interaction"), sad.GetComponent<BehaviorMecanim>().Node_StartInteraction(E, DD), new LeafWait(1000))
            );
    }

    protected Node ST_Gesture(string name, bool s)
    {
        Val<string> Gesture_name = Val.V(() => name);
        Val<bool> start = Val.V(() => s);
        return Daniel.GetComponent<BehaviorMecanim>().Node_BodyAnimation(Gesture_name, start);
    }
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

    protected Node ST_ThrowBall(GameObject P1, GameObject P2)
	{
		//Val<string> N = Val.V(() => name);
		//Val<bool> S = Val.V (() => start);
		//Val<Vector3> D = Val.V(() => direction);

        return new SequenceParallel(new LeafTrace("throwBall"),
                            new Sequence(new LeafWait(2500), ball_to_throw.GetComponent<ball>().Node_throw(P1.transform.position - P2.transform.position)), 
                            new Sequence(this.ST_Gesture("throw", true),new LeafWait(2000),this.ST_Gesture("throw",false)));
	}

    protected Node ST_ClickButton(GameObject clicker, FullBodyBipedEffector effector, InteractionObject button)
    {
        Val<FullBodyBipedEffector> E = Val.V(() => effector);
        Val<InteractionObject> B = Val.V(() => button);
        return new Sequence(new LeafTrace("Interaction"), clicker.GetComponent<BehaviorMecanim>().Node_StartInteraction(E, B), new LeafWait(1000));
    }

    protected Node ST_Wander(GameObject participant)
    {
        return new DecoratorLoop(
            new Sequence(
                         this.ST_ApproachAndWait(participant,wander1 ),
                         this.ST_ApproachAndWait(participant,wander2 ),
                         this.ST_ApproachAndWait(participant,wander3 )
             )
             );
    }

    protected Node ST_Face_to_Face(GameObject A, GameObject B)
    {
        Val<Vector3> A1 = Val.V(() => A.transform.position);
        Val<Vector3> B1 = Val.V(() => B.transform.position);
        return new SequenceParallel(new LeafTrace("face_to_face"), B.GetComponent<BehaviorMecanim>().Node_OrientTowards(A1), A.GetComponent<BehaviorMecanim>().Node_OrientTowards(B1));
    }

    protected Node ST_Converse_2_person(GameObject person1, GameObject person2, int loop)
    {
        return new Sequence(
            this.ST_Face_to_Face(person1, person2),
            new DecoratorLoop(loop,
            new Sequence(
                  new SelectorRandom(
                    //this.ST_Gestures_Body(person1, "throw", 1000),
                    this.ST_Gestures_Hand(person1, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person1, "cheer", 1000)
                    ),
                     new SelectorRandom(
                    //this.ST_Gestures_Body(person2, "throw", 1000),
                    this.ST_Gestures_Hand(person2, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person2, "cheer", 1000)
                    )
                   )
                   )

            );
    }
    protected Node ST_Converse_3_person(GameObject person1, GameObject person2,GameObject person3, int loop)
    {
        
          return new DecoratorLoop(loop,
           new Sequence(
                    new SelectorRandom(
                    //this.ST_Gestures_Body(person1, "throw", 1000),
                    this.ST_Gestures_Hand(person1, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person1, "cheer", 1000)
                    ),
                     new SelectorRandom(
                    //this.ST_Gestures_Body(person2, "throw", 1000),
                    this.ST_Gestures_Hand(person2, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person2, "cheer", 1000)
                    ),
                      new SelectorRandom(
                    //this.ST_Gestures_Body(person3, "throw", 1000),
                    this.ST_Gestures_Hand(person3, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person3, "cheer", 1000)
                    )
                    )

            );
    }
    protected Node ST_Converse_2_person(GameObject person1, GameObject person2)
    {
        return new Sequence(
            this.ST_Face_to_Face(person1, person2),
            new Sequence(
                     new SelectorRandom(
                    //this.ST_Gestures_Body(person1, "throw", 1000),
                    this.ST_Gestures_Hand(person1, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person1, "cheer", 1000)
                    ),
                     new SelectorRandom(
                    //this.ST_Gestures_Body(person2, "throw", 1000),
                    this.ST_Gestures_Hand(person2, "beingcocky", 1000),
                    this.ST_Gestures_Hand(person2, "cheer", 1000)
                   )

            ));
    }

    protected Node ST_Runaway_hide(GameObject P, Vector3 position)
    {
        Val<Vector3> target = Val.V(() => position);
        return new Sequence(P.GetComponent<BehaviorMecanim>().Node_GoTo(target), new LeafWait(1000));
    }

    protected Node ST_BallArc()
    {
        return new Sequence(
                                this.ST_ApproachAndWait(Daniel, conversePosition),
                                this.ST_Face_to_Face(Daniel, converse_2),
                                this.ST_Face_to_Face(Daniel, converse_1),
                                this.ST_Converse_3_person(Daniel, converse_2, converse_1, 3),
                                this.ST_ApproachAndWait(Daniel, ballPosition),
                                this.ST_PickUpBall(Daniel, Effector, Ball),
                                this.ST_ApproachAndWait(Daniel, ThrowPosition),
                                this.ST_Face_to_Face(converse_1, Daniel),
                                this.ST_ThrowBall(throwDirection, Daniel),
                                this.ST_PickUpBall(converse_1, Effector, Ball),
                                this.ST_Converse_3_person(Daniel, converse_2, converse_1, 1),
                                this.ST_Gestures_Hand(converse_2, "pointing", 1000)
                                );
    }
    protected Node ST_SadArc()
    {
       return new Sequence(
                               this.ST_Gestures_Face(sadperson, "sad", 1000),
                               this.ST_ApproachAndWait(Daniel, sadPosition),
                               this.ST_Converse_2_person(Daniel, sadperson, 1),
                               this.ST_Face_to_Face(sadperson, Peter),
                               this.ST_Gestures_Hand(sadperson, "pointing", 1000),
                               this.ST_Face_to_Face(sadperson, Daniel),
                               this.ST_shakehand(sadperson, Daniel, shakehand, Daniel_righthand, sad_righthand)
                               );
    }
    protected Node BuildTreeRoot()
    {
        return
              new DecoratorLoop(-1,
                new Sequence(
                        this.ST_Converse_2_person(Daniel, Peter, 2),
                        this.ST_Runaway_hide(Peter, ButtonPosition.position),
                        this.ST_ClickButton(Peter, Effector, Button),
                        this.ST_Runaway_hide(Peter, HidePosition.position),
                        this.ST_ApproachAndWait(Daniel, make_choice_position),
                        new SequenceParallel(
                            this.ST_Converse_2_person(converse_1, converse_2),
                            this.ST_Gestures_Face(sadperson,"sad",1000)),
                        new SelectorRandom(
                            new Sequence(
                                this.ST_BallArc(),
                                this.ST_SadArc()),
                            this.ST_SadArc()
                                    ),
                        this.ST_ApproachAndWait(Daniel,nearHidePosition),
                        this.ST_Face_to_Face(Peter,Daniel),
                        this.ST_Converse_2_person(Peter,Daniel,-1),
                        new LeafWait(100000)
                        )
                        );
    }

    public bool Dianl_arrive_choice()
    {
        return !arrive_at_choice;
    }

}
