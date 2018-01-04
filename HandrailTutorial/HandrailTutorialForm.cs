using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace HandrailTutorial
{
	public partial class HandrailTutorialForm : Form
	{
		private Model _model = new Model();
		private Picker _picker = new Picker();

		public HandrailTutorialForm()
		{
			InitializeComponent();
		}

		private void HandrailTutorialForm_Load(object sender, EventArgs e)
		{

		}

		private void buttonSelectBeam_Click(object sender, EventArgs e)
		{
			try
			{
				var beam = _picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Beam;

				var handrailStandard = new HandrailStandard
				{
					Side = comboBoxHandrailSide.Text
				};

				var handrail = new Handrail(handrailStandard, beam);
				handrail.Create();

				_model.CommitChanges();
			}
			catch (Exception ex)
			{

			}
		}
	}

	public class Handrail
	{
		private readonly HandrailStandard _handrailStandard;
		private readonly Beam _beam;
		private readonly List<Beam> _posts = new List<Beam>();
		private List<Beam> _kickPlates = new List<Beam>();
		private List<Beam> _topRails = new List<Beam>();
		private List<Beam> _midRails = new List<Beam>();
		private Assembly _assembly;

		public Handrail(HandrailStandard handrailStandard, Beam beam)
		{
			_handrailStandard = handrailStandard;
			_beam = beam;
		}

		public void Create()
		{
			var start = _beam.StartPoint;
			var end = _beam.EndPoint;

			if (_handrailStandard.Side == "LEFT")
			{
				start = _beam.EndPoint;
				end = _beam.StartPoint;
			}

			var width = 0.0;
			_beam.GetReportProperty("WIDTH", ref width);
			var planeOffset = width / 2 + _handrailStandard.PostGap;

			//posts
			var length = Distance.PointToPoint(start, end);
			var numberOfPosts = int.Parse(Math.Round(length / _handrailStandard.MaxPostSpacing, MidpointRounding.AwayFromZero).ToString()) + 1;

			var vector = new Vector
			{
				X = (end.X - start.X),
				Y = (end.Y - start.Y),
				Z = (end.Z - start.Z)
			};

			var angle = Math.Round(vector.GetAngleBetween(new Vector(1, 0, 0)) / (Math.PI / 180), 1);

			var postSpacing = (length - (2 * _handrailStandard.EndClearance)) / (numberOfPosts - 1);

			var startPoint = vector.GetNormal() as Point;

			startPoint.X = startPoint.X * _handrailStandard.EndClearance;
			startPoint.Y = startPoint.Y * _handrailStandard.EndClearance;

			startPoint = startPoint + start;

			var angle1 = _handrailStandard.Side == "LEFT" ? -angle : angle;

			for (int i = 0; i <= numberOfPosts - 1; i++)
			{
				var point = new Point();
				if (i == 0)
				{
					point = startPoint;
				}
				else
				{
					startPoint = vector.GetNormal();

					startPoint.X = startPoint.X * (_handrailStandard.EndClearance + (i * postSpacing));
					startPoint.Y = startPoint.Y * (_handrailStandard.EndClearance + (i * postSpacing));

					point = startPoint + start;
				}

				var post = new Beam
				{
					Profile = new Profile { ProfileString = _handrailStandard.PostProfile },
					StartPoint = point,
					EndPoint = new Point(point.X, point.Y, point.Z + _handrailStandard.TopRailAboveSteel),
					Position = new Position
					{
						Depth = Position.DepthEnum.BEHIND,
						DepthOffset = 0,
						Plane = Position.PlaneEnum.RIGHT,
						PlaneOffset = planeOffset,
						Rotation = Position.RotationEnum.BACK,
						RotationOffset = angle1,
					}
				};

				_posts.Add(post);
			}

			var railWebThickness = 6.4;
			var vector2 = vector.GetNormal() as Point;

			//toprail
			var topRail = new Beam
			{
				Profile = new Profile { ProfileString = _handrailStandard.TopRailProfile },
				StartPoint = _posts[0].EndPoint,
				EndPoint = new Point(
					_posts[_posts.Count - 1].EndPoint.X + (vector2.X * 76),
					_posts[_posts.Count - 1].EndPoint.Y + (vector2.Y * 76),
					_posts[_posts.Count - 1].EndPoint.Z),
				Position = new Position
				{
					Depth = Position.DepthEnum.BEHIND,
					DepthOffset = 0,
					Plane = Position.PlaneEnum.RIGHT,
					PlaneOffset = planeOffset - railWebThickness,
					Rotation = Position.RotationEnum.BACK,
					RotationOffset = 0
				}
			};

			_topRails.Add(topRail);

			//midrail
			var midStartPoint = new Point(_posts[0].StartPoint);
			midStartPoint.Z = midStartPoint.Z + _handrailStandard.MidRailAboveSteel;
			var midEndPoint = new Point(
				_posts[_posts.Count - 1].StartPoint.X + (vector2.X * 76),
				_posts[_posts.Count - 1].StartPoint.Y + (vector2.Y * 76),
				_posts[_posts.Count - 1].StartPoint.Z + _handrailStandard.MidRailAboveSteel);

			var midRail = new Beam
			{
				Profile = new Profile { ProfileString = _handrailStandard.MidRailProfile },
				StartPoint = midStartPoint,
				EndPoint = midEndPoint,
				Position = new Position
				{
					Depth = Position.DepthEnum.BEHIND,
					DepthOffset = 0,
					Plane = Position.PlaneEnum.RIGHT,
					PlaneOffset = planeOffset - railWebThickness,
					Rotation = Position.RotationEnum.BACK,
					RotationOffset = 0
				}
			};

			_midRails.Add(midRail);

			//kickplate
			var kpStartPoint = new Point(_posts[0].StartPoint);
			var kpEndPoint = new Point(
				_posts[_posts.Count - 1].StartPoint.X + (vector2.X * 76),
				_posts[_posts.Count - 1].StartPoint.Y + (vector2.Y * 76),
				_posts[_posts.Count - 1].StartPoint.Z);

			var kickPlate = new Beam
			{
				Profile = new Profile { ProfileString = _handrailStandard.KickplateProfile },
				StartPoint = kpStartPoint,
				EndPoint = kpEndPoint,
				Position = new Position
				{
					Depth = Position.DepthEnum.BEHIND,
					DepthOffset = _handrailStandard.KickplateAboveSteel + _handrailStandard.FloorThickness,
					Plane = Position.PlaneEnum.RIGHT,
					PlaneOffset = -planeOffset,
					Rotation = Position.RotationEnum.TOP,
					RotationOffset = 180
				}
			};

			_kickPlates.Add(kickPlate);

			//insert members
			_posts.ForEach(p => p.Insert());
			_topRails.ForEach(p => p.Insert());
			_midRails.ForEach(p => p.Insert());
			_kickPlates.ForEach(p => p.Insert());

			//create assembly
			_assembly = _topRails[0].GetAssembly();
			_topRails.ForEach(p => _assembly.Add(p));
			_midRails.ForEach(p => _assembly.Add(p));
			_kickPlates.ForEach(p => _assembly.Add(p));
			_posts.ForEach(p => _assembly.Add(p));

			var result = _assembly.Modify();

			//connections
			_posts.ForEach(p =>
			{
				var connection = new Connection
				{
					Name = _handrailStandard.PostToSupportConnection.Name,
					Number = _handrailStandard.PostToSupportConnection.Number,
				};

				connection.LoadAttributesFromFile("standard");
				InsertConnection(connection, _beam, p);
			});
		}

		private void InsertConnection(Connection connection, Beam primary, Beam secondary)
		{
			connection.SetPrimaryObject(primary);
			connection.SetSecondaryObject(secondary);

			if (!connection.Insert())
			{
				Console.WriteLine("Connection insert failed");
			}
		}
	}

	public class HandrailStandard
	{
		public int NumberOfPosts { get; set; }
		public int PostSpacing { get; set; }
		public int MinPostSpacing { get; set; }
		public int MaxPostSpacing { get; set; }
		public string PostProfile { get; set; }
		public int PostGap { get; set; }
		public bool HasKickplate { get; set; }
		public string KickplateProfile { get; set; }
		public string MidRailProfile { get; set; }
		public string TopRailProfile { get; set; }
		public int KickplateAboveSteel { get; set; }
		public int FloorThickness { get; set; }
		public int TopRailAboveSteel { get; set; }
		public int MidRailAboveSteel { get; set; }
		public int MaxEndLoopDistance { get; set; }
		public int MinEndLoopDistance { get; set; }
		public int EndClearance { get; set; }
		public string Side { get; set; }
		public int PostTopGap { get; set; }
		public Connection PostToSupportConnection { get; set; }

		public HandrailStandard()
		{
			NumberOfPosts = 2;
			PostSpacing = 1200;
			MinPostSpacing = 300;
			MaxPostSpacing = 1800;
			PostProfile = "L76X76X9.5";
			PostGap = 6;
			HasKickplate = true;
			KickplateProfile = "PL9.5X101.6";
			MidRailProfile = "L76X76X6.4";
			TopRailProfile = "L76X76X6.4";
			KickplateAboveSteel = 6;
			FloorThickness = 38;
			TopRailAboveSteel = 900;
			MidRailAboveSteel = 500;
			MaxEndLoopDistance = 300;
			MinEndLoopDistance = 150;
			EndClearance = 300;
			Side = "LEFT";
			PostTopGap = 50;

			PostToSupportConnection = new Connection()
			{
				Name = "Stringer stanchion Lprof",
				Number = 68,
			};
		}
	}

}
