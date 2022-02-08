using UnityEngine;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Conform")]
	public class MegaConformMod : MegaModifier
	{
		// Will have multiple in the end or layer
		public GameObject	target;
		public float[]		offsets;
		public Collider		conformCollider;
		public Bounds		bounds;
		public float[]		last;
		public Vector3[]	last1;
		public Vector3[]	conformedVerts;
		public float		conformAmount	= 1.0f;
		public float		raystartoff		= 0.0f;
		public float		offset			= 0.0f;
		public float		raydist			= 100.0f;
		public MegaAxis		axis			= MegaAxis.Y;
		Matrix4x4			loctoworld;
		Matrix4x4			ctm;
		Matrix4x4			cinvtm;
		Ray					ray				= new Ray();
		RaycastHit			hit;
		public bool			useLocalDown	= false;
		public bool			flipDown		= true;
		public MegaAxis		downAxis		= MegaAxis.Y;

		public override string ModName()	{ return "Conform"; }
		public override string GetHelpURL() { return "?page_id=4547"; }

		public void SetTarget(GameObject targ)
		{
			target = targ;

			if ( target )
				conformCollider = target.GetComponent<Collider>();
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			return p;
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( conformCollider )
			{
				if ( useLocalDown )
				{
					Vector3 down = Vector3.down;
					switch ( downAxis )
					{
						case MegaAxis.X:	down = transform.right;		break;
						case MegaAxis.Y:	down = transform.up;		break;
						case MegaAxis.Z:	down = transform.forward;	break;
					}

					if ( flipDown )
						down = -down;

					ray.direction = down;

					Vector3 rso = -down * raystartoff;

					Vector3 dir = ray.direction;
					Vector3 ldir = -transform.InverseTransformDirection(dir);

					for ( int i = 0; i < jverts.Length; i++ )
					{
						Vector3 origin = ctm.MultiplyPoint(jverts[i]) - rso;
						ray.origin = origin;

						jsverts[i] = jverts[i];

						if ( conformCollider.Raycast(ray, out hit, raydist) )
						{
							Vector3 lochit = cinvtm.MultiplyPoint(hit.point);

							jsverts[i] = Vector3.Lerp(jverts[i], lochit + (ldir * (offsets[i] + offset)), conformAmount);
							last1[i] = jsverts[i];
						}
						else
							jsverts[i] = last1[i];
					}
				}
				else
				{
					int ax = (int)axis;

					for ( int i = 0; i < jverts.Length; i++ )
					{
						Vector3 origin = ctm.MultiplyPoint(jverts[i]);
						origin.y += raystartoff;
						ray.origin = origin;
						ray.direction = Vector3.down;

						jsverts[i] = jverts[i];

						if ( conformCollider.Raycast(ray, out hit, raydist) )
						{
							Vector3 lochit = cinvtm.MultiplyPoint(hit.point);

							float v = Mathf.Lerp(jverts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);
							Vector3 vt = jsverts[i];
							vt[ax] = v;
							jsverts[i] = vt;	//[ax] = Mathf.Lerp(jverts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);
							last[i] = jsverts[i][ax];
						}
						else
						{
							Vector3 vt = jsverts[i];
							vt[ax] = last[i];
							jsverts[i] = vt;
						}
					}
				}
			}
			else
				jverts.CopyTo(jsverts);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( target )
			{
				if ( conformCollider != target.GetComponent<Collider>() )
					conformCollider = target.GetComponent<Collider>();

				if ( conformCollider == null )
					return false;

				if ( conformedVerts == null || conformedVerts.Length != mc.mod.jverts.Length )
				{
					conformedVerts = new Vector3[mc.mod.jverts.Length];
					// Need to run through all the source meshes and find the vertical offset from the base

					offsets = new float[mc.mod.jverts.Length];
					last = new float[mc.mod.jverts.Length];

					for ( int i = 0; i < mc.mod.jverts.Length; i++ )
						offsets[i] = mc.mod.jverts[i][(int)axis] - mc.bbox.min[(int)axis];
				}

				if ( useLocalDown && (last1 == null || last1.Length != last.Length) )
				{
					last1 = new Vector3[last.Length];
				}

				loctoworld = transform.localToWorldMatrix;

				ctm = loctoworld;
				cinvtm = transform.worldToLocalMatrix;

				return true;
			}
			else
				conformCollider = null;

			return true;
		}

		public void ChangeAxis()
		{
			MegaModifyObject mod = GetComponent<MegaModifyObject>();

			if ( mod )
			{
				for ( int i = 0; i < mod.jverts.Length; i++ )
					offsets[i] = mod.jverts[i][(int)axis] - mod.bbox.min[(int)axis];
			}
		}
	}
}