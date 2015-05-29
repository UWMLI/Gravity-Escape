using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GhostTrail : MonoBehaviour {
    public int line_point_count = 0;
	public float line_width = 0.2F;

	public Vector3 last_position;
	public LineRenderer current_line;
	public List<LineRenderer> line_segments = new List<LineRenderer>();

	public float last_point_time = 0.0F;

	public Material off_color;
	public Material thrust_on_color;
	public Material deThrust_on_color;
	bool new_segments_allowed = true;

    void Start()
	{
		current_line = new_line(off_color);
    }


    void Update()
	{
		// Thrust vs Glide
        if (should_thrust_segment_on ()) { current_line = new_line(thrust_on_color); }
        if (should_deThrust_segment_on()) { current_line = new_line(deThrust_on_color); }
        

        if (should_segment_off()) { current_line = new_line(off_color); }

		track_position();
    }


	// On and Off line events

	bool should_thrust_segment_on()  { return Input.GetButtonDown("Fire1") && new_segments_allowed; }
	bool should_deThrust_segment_on() { return Input.GetButtonDown("DeThrust") && new_segments_allowed;}

	bool should_segment_off() { return (Input.GetButtonUp ("Fire1") || Input.GetButtonUp("DeThrust")) && new_segments_allowed; }


	// Crete line segments

	public LineRenderer new_line(Material line_color)
	{
		GameObject line_object = new GameObject("Empty");

        LineRenderer line_renderer = line_object.AddComponent<LineRenderer>();

		line_renderer.material = line_color;
        line_renderer.SetWidth(line_width, line_width);

		// Reset for new line
		line_point_count = 0;
        line_renderer.SetVertexCount(line_point_count);

		// position of last trail end point
		if(line_segments.Count > 0)
		{
			line_point_count = 1;
			line_renderer.SetVertexCount(line_point_count);
			line_renderer.SetPosition(line_point_count-1, last_position);
		}

		// List of line segments for this object
		line_segments.Add(line_renderer);

		return line_renderer;
	}


	// Track parent location

	void track_position()
	{
		if(should_track())
		{
			Vector3 position = new Vector3(transform.position.x, transform.position.y);

			last_position = position;

			line_point_count += 1;
			current_line.SetVertexCount(line_point_count);
			current_line.SetPosition(line_point_count-1, position);
		}
	}


	bool should_track()
	{
		if(Time.time - last_point_time > 0.001F)
		{
			last_point_time = Time.time;
			return true;
		}
		return false;
	}


	// External control of new segments

	public void disable_new_segments()
	{
		new_segments_allowed = false;
		current_line = new_line(off_color);
	}

	public void enable_new_segments()
	{
		new_segments_allowed = true;

		// Add new one if finger down already
		if(Input.GetButtonDown("Fire1"))
		{
			current_line = new_line(thrust_on_color);
		}
		if(Input.GetButtonDown("DeThrust")){
			current_line = new_line(deThrust_on_color);
		}
	}
}
