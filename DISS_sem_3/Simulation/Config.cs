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
    public static readonly PointF ENTRY_QUEUE_POSITION = new PointF(800, 550);
    public static readonly PointF MEDICAL_QUEUE_A_POSITION = new PointF(800, 600);
    public static readonly PointF MEDICAL_QUEUE_B_POSITION = new PointF(800, 650);
    public const float HALLWAY_Y = 500;

    public static readonly PointF[] PATH_AMBULANCE_ENTRY_TO_QUEUE = new PointF[]
    {
        AMBULANCE_ENTRANCE_POSITION, // Start
        new PointF(655, 550), // Enter Corridor
        ENTRY_QUEUE_POSITION
    };
    public static readonly PointF[] PATH_WALK_IN_ENTRY_TO_QUEUE = new PointF[]
    {
        EXIT_ENTRANCE_POSITION, // Start
        new PointF(655, 550), // Enter Corridor
        new PointF(800, 550)
    };

    public static readonly PointF ROOM_B_0 = new PointF(220, 140);
    public static readonly PointF ROOM_B_1 = new PointF(1050, 140);
    public static readonly PointF ROOM_B_2 = new PointF(1350, 140);
    public static readonly PointF ROOM_B_3 = new PointF(1650, 140);
    public static readonly PointF ROOM_B_4 = new PointF(1050, 995);
    public static readonly PointF ROOM_B_5 = new PointF(1350, 995);
    public static readonly PointF ROOM_B_6 = new PointF(1650, 995);
    
    public static readonly PointF ROOM_A_0 = new PointF(220, 350);
    public static readonly PointF ROOM_A_1 = new PointF(220, 560);
    public static readonly PointF ROOM_A_2 = new PointF(220, 780);
    public static readonly PointF ROOM_A_3 = new PointF(220, 995);
    public static readonly PointF ROOM_A_4 = new PointF(220, 1210);
    
    public static readonly PointF[][] PATH_ROOM_B_EXIT = new PointF[][]
    {
        // ROOM B0
        new PointF[]{ 
            ROOM_B_0,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B1
        new PointF[]{ 
            ROOM_B_1,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B2
        new PointF[]{ 
            ROOM_B_2,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B3
        new PointF[]{
            ROOM_B_3,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B4
        new PointF[]{ 
            ROOM_B_4,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B5
        new PointF[]{
            ROOM_B_5,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM B6
        new PointF[]{
            ROOM_B_6, 
            EXIT_ENTRANCE_POSITION           
        }
    };
    public static readonly PointF[][] PATH_ROOM_A_EXIT = new PointF[][]
    {
        // ROOM A0
        new PointF[]{ 
            ROOM_A_0,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM A1
        new PointF[]{ 
            ROOM_A_1,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM A2
        new PointF[]{ 
            ROOM_A_2,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM A3
        new PointF[]{
            ROOM_A_3,
            EXIT_ENTRANCE_POSITION
        },
        //ROOM A4
        new PointF[]{ 
            ROOM_A_4,
            EXIT_ENTRANCE_POSITION
        }
    };
    public static readonly PointF[][] PATH_ENTRY_QUEUE_TO_ROOM_B = new PointF[][]
    {
        // ROOM B0
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(655, 550),
            new PointF(655, 140),
            ROOM_B_0            
        },
        //ROOM B1
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1050, 550),
            ROOM_B_1            
        },
        //ROOM B2
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1350, 550),
            ROOM_B_2            
        },
        //ROOM B3
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1650, 550),
            ROOM_B_3            
        },
        //ROOM B4
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1050, 550),
            ROOM_B_4            
        },
        //ROOM B5
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1350, 550),
            ROOM_B_5            
        },
        //ROOM B6
        new PointF[]{ 
            ENTRY_QUEUE_POSITION,  
            new PointF(1650, 550),
            ROOM_B_6            
        }
    };
    public static readonly PointF[][] PATH_MEDICAL_STAFF_TO_ROOM_B = new PointF[][]
    {
        // Room B 0
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,                
            ROOM_B_0            
        },
        // Room B 1
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,                  
            new PointF(655, 550),      
            ROOM_B_1           
        },
        // Room B 2
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(655, 550),      
            ROOM_B_2  
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(655, 550),      
            ROOM_B_3  
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(655, 550),      
            ROOM_B_4  
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(655, 550),      
            ROOM_B_5
        },new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION, 
            new PointF(655, 550),      
            ROOM_B_6  
        }
    };
    
    public static readonly PointF[][] PATH_MEDICAL_STAFF_TO_ROOM_A = new PointF[][]
    {
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,  
            ROOM_A_0            
        },
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,  
            new PointF(655, 560),
            ROOM_A_1            
        },
        // ROOM A2
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,  
            new PointF(655, 780),
            ROOM_A_2            
        },
        // ROOM A3
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,  
            new PointF(655, 995),
            ROOM_A_3            
        },
        // ROOM A4
        new PointF[]{ 
            AMBULANCE_ENTRANCE_POSITION,  
            new PointF(655, 1210),
            ROOM_A_4            
        }
    };
    public static readonly PointF[][] PATH_MEDICAL_A_QUEUE_TO_ROOM_A = new PointF[][]
    {
        // ROOM A0
        new PointF[]{ 
            MEDICAL_QUEUE_A_POSITION,  
            ROOM_A_0            
        },
        // ROOM A1
        new PointF[]{ 
            MEDICAL_QUEUE_A_POSITION,  
            ROOM_A_1            
        },
        // ROOM A2
        new PointF[]{ 
            MEDICAL_QUEUE_A_POSITION,
            ROOM_A_2            
        },
        // ROOM A3
        new PointF[]{ 
            MEDICAL_QUEUE_A_POSITION,
            ROOM_A_3            
        },
        // ROOM A4
        new PointF[]{ 
            MEDICAL_QUEUE_A_POSITION,
            ROOM_A_4            
        }
    };
    public static readonly PointF[][] PATH_MEDICAL_B_QUEUE_TO_ROOM_A = new PointF[][]
    {
        // ROOM A0
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,  
            ROOM_A_0            
        },
        // ROOM A1
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,  
            ROOM_A_1            
        },
        // ROOM A2
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_A_2            
        },
        // ROOM A3
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_A_3            
        },
        // ROOM A4
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_A_4            
        }
    };
    public static readonly PointF[][] PATH_MEDICAL_B_QUEUE_TO_ROOM_B = new PointF[][]
    {
        // ROOM B0
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,  
            ROOM_B_0            
        },
        // ROOM B1
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,  
            ROOM_B_1            
        },
        // ROOM B2
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_B_2            
        },
        // ROOM B3
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_B_3            
        },
        // ROOM B4
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_B_4            
        },
        // ROOM B5
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_B_5            
        },
        // ROOM B6
        new PointF[]{ 
            MEDICAL_QUEUE_B_POSITION,
            ROOM_B_6            
        }
    };
}