using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeformableMesh : MonoBehaviour {

	/*
	 *  Members
	 */


	private Mesh			_mesh;
	private Material		_material;
	private List<Vector3>	_points;
	private List<Vector3>	_vertices;
	private List<int>		_triangles;
	private List<Vector2>	_UVs;

	private float			_size = .5f;

	/*
	 * Methods
	 */

	void Start () {
		_points = new List<Vector3> ( );

		_points.Add ( new Vector3 (-_size,  _size, -_size ) );
		_points.Add ( new Vector3 ( _size,  _size, -_size ) );
		_points.Add ( new Vector3 ( _size,  -_size, -_size ) );
		_points.Add ( new Vector3 ( -_size, -_size, -_size ) );

		_points.Add ( new Vector3 ( _size,  _size, _size ) );
		_points.Add ( new Vector3 ( -_size, _size, _size ) );
		_points.Add ( new Vector3 ( -_size, -_size, _size ) );
		_points.Add ( new Vector3 ( _size,  -_size, _size ) );

		_vertices	= new List<Vector3> ( );
		_triangles	= new List<int> ( );
		_UVs		= new List<Vector2> ( );		

		CreatMesh ( );
	}
	
	
	void Update () {
		if ( Input.GetMouseButton ( 0 ) ) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );

			if ( Physics.Raycast ( ray, out hit ) ) {
				RaisePoint ( FindNearestPoint ( hit.point ) );
			}
		}

		if ( Input.GetMouseButton ( 1 ) ) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );

			if ( Physics.Raycast ( ray, out hit ) ) {
				LowerPoint ( FindNearestPoint ( hit.point ) );
			}
		}
	}


	private Vector3 FindNearestPoint ( Vector3 point ) {
		Vector3 nearestPoint = new Vector3 ( );
		float lastDistance = 9999999f;

		for ( int i= 0; i < _points.Count; i++ ) {
			float distance = GetDistance ( point,  _points[i] );
			if ( distance < lastDistance ) {
				lastDistance = distance;
				nearestPoint = _points[i];
			}
		}

		return nearestPoint;
	}


	private float GetDistance ( Vector3 start, Vector3 end ) {
		return Mathf.Sqrt ( Mathf.Pow ( ( start.x - end.x ), 2 ) + Mathf.Pow ( ( start.y - end.y ), 2 ) + Mathf.Pow ( ( start.z - end.z ), 2 ) );
	}


	private void RaisePoint ( Vector3 point ) {
		int index = -1;

		for ( int i  = 0; i < _points.Count; i++ ) {
			if ( _points[i] == point ) {
				index = i;
				break;
			}
		}

		if ( index == -1 )
			Debug.LogError ( "Could not match point." );
		else {
			Vector3 newPoint = _points[index];
			newPoint.y += .01f;

			_points[index] = newPoint;

			UpdateMesh ( );
		}
	}


	private void LowerPoint ( Vector3 point ) {
		int index = -1;

		for ( int i  = 0; i < _points.Count; i++ ) {
			if ( _points[i] == point ) {
				index = i;
				break;
			}
		}

		if ( index == -1 )
			Debug.LogError ( "Could not match point." );
		else {
			Vector3 newPoint = _points[index];
			newPoint.y -= .01f;

			_points[index] = newPoint;

			UpdateMesh ( );
		}
	}


	private void CreatMesh ( ) {
		gameObject.AddComponent <MeshFilter>(  );
		gameObject.AddComponent <MeshRenderer>(  );
		gameObject.AddComponent <MeshCollider>(  );

		_material = Resources.Load ( "Materials/Default" ) as Material;
		if ( _material == null ) {
			Debug.LogError ( "Material not found." );
			return;
		}

		MeshFilter meshFilter = GetComponent<MeshFilter> ( );
		if ( meshFilter == null ) {
			Debug.LogError ( "Mesh Filter not found." );
			return;
		}

		_mesh = meshFilter.sharedMesh;
		if ( _mesh == null ) {
			meshFilter.mesh = new Mesh ( );
			_mesh = meshFilter.sharedMesh;
		}

		MeshCollider meshCollider = GetComponent<MeshCollider> ( );
		if ( meshCollider == null ) {
			Debug.LogError ( "Mesh Collider not found." );
			return;
		}

		_mesh.Clear ( );
		UpdateMesh ( );
	}


	private void UpdateMesh ( ) { 
		// Front plane
		_vertices.Add ( _points[0] ); _vertices.Add ( _points[1] ); _vertices.Add ( _points[2] ); _vertices.Add ( _points[3] );

		// Back plane
		_vertices.Add ( _points[4] ); _vertices.Add ( _points[5] ); _vertices.Add ( _points[6] ); _vertices.Add ( _points[7] );

		// Left plane
		_vertices.Add ( _points[5] ); _vertices.Add ( _points[0] ); _vertices.Add ( _points[3] ); _vertices.Add ( _points[6] );

		// Right plane
		_vertices.Add ( _points[1] ); _vertices.Add ( _points[4] ); _vertices.Add ( _points[7] ); _vertices.Add ( _points[2] );

		// Top plane
		_vertices.Add ( _points[5] ); _vertices.Add ( _points[4] ); _vertices.Add ( _points[1] ); _vertices.Add ( _points[0] );

		// Bot plane
		_vertices.Add ( _points[3] ); _vertices.Add ( _points[2] ); _vertices.Add ( _points[7] ); _vertices.Add ( _points[6] );



		// Front plane
		_triangles.Add ( 0 ); _triangles.Add ( 1 ); _triangles.Add ( 2 );
		_triangles.Add ( 2 ); _triangles.Add ( 3 ); _triangles.Add ( 0 );

		// Back plane
		_triangles.Add ( 4 ); _triangles.Add ( 5 ); _triangles.Add ( 6 );
		_triangles.Add ( 6 ); _triangles.Add ( 7 ); _triangles.Add ( 4 );

		// Left plane
		_triangles.Add ( 8 ); _triangles.Add ( 9 ); _triangles.Add ( 10 );
		_triangles.Add ( 10 ); _triangles.Add ( 11 ); _triangles.Add ( 8 );

		// Right plane
		_triangles.Add ( 12 ); _triangles.Add ( 13 ); _triangles.Add ( 14 );
		_triangles.Add ( 14 ); _triangles.Add ( 15 ); _triangles.Add ( 12 );

		// Top plane
		_triangles.Add ( 16 ); _triangles.Add ( 17 ); _triangles.Add ( 18 );
		_triangles.Add ( 18 ); _triangles.Add ( 19 ); _triangles.Add ( 16 );

		// Bot plane
		_triangles.Add ( 20 ); _triangles.Add ( 21 ); _triangles.Add ( 22 );
		_triangles.Add ( 22 ); _triangles.Add ( 23 ); _triangles.Add ( 20 );


		// Front plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		// Back plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		// Left plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		// Right plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		// Top plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		// Bot plane
		_UVs.Add ( new Vector2 ( 0, 1 ) );
		_UVs.Add ( new Vector2 ( 1, 0 ) );
		_UVs.Add ( new Vector2 ( 1, 1 ) );
		_UVs.Add ( new Vector2 ( 0, 0 ) );

		_mesh.vertices	= _vertices.ToArray ( );
		_mesh.triangles = _triangles.ToArray ( );
		_mesh.uv		= _UVs.ToArray ( );

		_vertices.Clear ( );
		_triangles.Clear ( );
		_UVs.Clear ( );

		MeshCollider meshCollider = GetComponent<MeshCollider> ( );
		
		_mesh.RecalculateNormals ( );
		_mesh.RecalculateBounds ( );

		RecalculateTangents ( _mesh );

		meshCollider.sharedMesh	= null;
		meshCollider.sharedMesh = _mesh;

		GetComponent<Renderer>().material = _material;

		_mesh.Optimize ( );
	}

	private void RecalculateTangents ( Mesh mesh ) {
		int[] triangles		= mesh.triangles;
		Vector3[] vertices	= mesh.vertices;
		Vector2[] uvs		= mesh.uv;
		Vector3[] normals	= mesh.normals;

		int triangleCount	= triangles.Length;
		int vertexCount		= vertices.Length;

		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];

		Vector4[] tangents =  new Vector4[vertexCount];

		for ( long a = 0; a < triangleCount; a += 3 ) {
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];

			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];

			Vector2 w1 = uvs[i1];
			Vector2 w2 = uvs[i2];
			Vector2 w3 = uvs[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float div = s1 * t2 - s2 * t1;
			float r = div == 0f ? 0f : 1.0F / div;

			Vector3 sdir = new Vector3 ( ( t2 * x1 - t1 * x2 ) * r, ( t2 * y1 - t1 * y2 ) * r, ( t2 * z1 - t1 * z2 ) * r );
			Vector3 tdir = new Vector3 ( ( s1 * x2 - s2 * x1 ) * r, ( s1 * y2 - s2 * y1 ) * r, ( s1 * z2 - s2 * z1 ) * r );

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}

		for ( long a = 0; a < vertexCount; ++a ) {
			Vector3 n = normals[a];
			Vector3 t = tan1[a];

			Vector3.OrthoNormalize ( ref n, ref t );
			
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;

			tangents[a].w = ( Vector3.Dot ( Vector3.Cross ( n, t ), tan2[a] ) < 0f ) ? -1.0f : 1.0f;
		}

		mesh.tangents = tangents;
	}
}
