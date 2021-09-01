using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Waypoint))] // the class which will be controlled by this editor
public class waypointEditor : Editor //not inherited from monobehaviour
{
    Waypoint waypoint => target as Waypoint; // casted to target of this editor to be a waypoint type

    private void OnSceneGUI() // handles will go on in our scene 
    {
        Handles.color = Color.red;
        for (int i = 0; i < waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            // creating handles
            Vector3 currentWaypointPoint = waypoint.currentPosition + waypoint.Points[i]; // we have position of each point in our waypoint
            Vector3 newWayPointPoint = Handles.FreeMoveHandle(currentWaypointPoint, Quaternion.identity, 0.7f,new Vector3(0.3f,0.3f,0.3f), Handles.SphereHandleCap); //first parameter is the position where this handles will be spawn

            //creating label text for the points in our path 
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;
            Vector3 textAlignment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(waypoint.currentPosition +waypoint.Points[i]+textAlignment,$"{i+1}",textStyle); //position,
            bool changeHappened = EditorGUI.EndChangeCheck(); // if change happens
            

            if (changeHappened) // if any change has happened in the scene, we are going to apply the changes 
            {
                // updating each position of each point
                Undo.RecordObject(target, name:"Free Move Handle"); // recording our target and adding a name 
                waypoint.Points[i] = newWayPointPoint - waypoint.currentPosition;  //minus startposition of waypoint // apply new position for each point.

            }

        }
    }

}
