using System.IO;

namespace Simulation;

public class Config
{
    //IMAGES
    public static readonly string BACKGROUND_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "background.jpg");
    public static readonly string DOCTOR_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "doctor.png");
    public static readonly string NURSE_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "nurse.png");
    public static readonly string WALK_IN_PATIENT_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "walkIn_patient.png");
    public static readonly string AMBULANCE_PATIENT_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ambulance_patient.png");
    public static readonly Image BACKGROUND_IMG;
    public static readonly Image DOCTOR_IMG;
    public static readonly Image NURSE_IMG;
    public static readonly Image WALK_IN_PATIENT_IMG;
    public static readonly Image AMBULANCE_PATIENT_IMG;
    
    static Config()
    {
        BACKGROUND_IMG = Image.FromFile(BACKGROUND_IMG_PATH);
        DOCTOR_IMG = Image.FromFile(DOCTOR_IMG_PATH);
        NURSE_IMG = Image.FromFile(NURSE_IMG_PATH);
        WALK_IN_PATIENT_IMG = Image.FromFile(WALK_IN_PATIENT_IMG_PATH);
        AMBULANCE_PATIENT_IMG = Image.FromFile(AMBULANCE_PATIENT_IMG_PATH);
    }
    
    //ENTRANCE
    public static readonly PointF AMBULANCE_ENTRANCE_POSITION = new PointF(655, 70);
    public static readonly PointF EXIT_ENTRANCE_POSITION = new PointF(655, 1200);
    public static readonly PointF BASE_POSITION_DOCTORS = new PointF(555, 70);
    public const float HALLWAY_Y = 500;

    public static readonly PointF[] PATH_AMBULANCE_ENTRY_TO_QUEUE = new PointF[]
    {
        AMBULANCE_ENTRANCE_POSITION, // Start
        new PointF(655, 550), // Enter Corridor
        new PointF(800, 550)
    };
    public static readonly PointF[] PATH_WALK_IN_ENTRY_TO_QUEUE = new PointF[]
    {
        EXIT_ENTRANCE_POSITION, // Start
        new PointF(655, 550), // Enter Corridor
        new PointF(800, 550)
    };

    public static readonly PointF ROOM_B_0 = new PointF(266, 139);
    
    public static readonly PointF[][] PATH_ENTRY_QUEUE_TO_ROOM_B = new PointF[][]
    {
        // IZBA B0
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,         
            ROOM_B_0            
        },
        // IZBA A2
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,                   // Start
            new PointF(1161, HALLWAY_Y),       // Enter Corridor
            new PointF(525, HALLWAY_Y),        // Move to Room A2 X-position
            new PointF(525, 139)               // Enter Room A2
        },
        // IZBA A3
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },
        // Add more rooms following the same pattern...
    };
    
    public static readonly PointF[][] PATH_MEDICAL_STAFF_TO_ROOM = new PointF[][]
    {
        // IZBA A1
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,                   // Start
            new PointF(1161, HALLWAY_Y),       // Enter Corridor
            new PointF(266, HALLWAY_Y),        // Move to Room A1 X-position
            new PointF(266, 139)               // Enter Room A1
        },
        // IZBA A2
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,                   // Start
            new PointF(1161, HALLWAY_Y),       // Enter Corridor
            new PointF(525, HALLWAY_Y),        // Move to Room A2 X-position
            new PointF(525, 139)               // Enter Room A2
        },
        // IZBA A3
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(1161, HALLWAY_Y), 
            new PointF(783, HALLWAY_Y), 
            new PointF(783, 139) 
        },
        // Add more rooms following the same pattern...
    };
}