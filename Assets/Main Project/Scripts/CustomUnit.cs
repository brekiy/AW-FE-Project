using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnit : Unit
{

    /*// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public Color LeadingColor;
    public override void Initialize()
    {
        base.Initialize();
        transform.position += new Vector3(0, 0, -1);
        GetComponent<Renderer>().material.color = LeadingColor;
    }

    /*public void MarkAsActive()
    {
        GetComponent<Renderer>().material.color = LeadingColor;
    }*/

    public override void MarkAsFinished()
    {
        GetComponent<Renderer>().material.color = LeadingColor + new Color(0.8f, 0.8f, 0.8f);
    }

    public override void MarkAsAttacking(Unit other){

    }

    public override void MarkAsDefending(Unit other)
    {

    }

    public override void MarkAsDestroyed(){

    }

    public override void MarkAsFriendly()
    {
        GetComponent<Renderer>().material.color = LeadingColor + new Color(0.8f, 1, 0.8f);
    }

    public override void MarkAsReachableEnemy()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.red;
    }

    public override void MarkAsSelected()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.green;
    }

    public override void UnMark()
    {
        GetComponent<Renderer>().material.color = LeadingColor;
    }
}