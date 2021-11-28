using System.Collections.Generic;

namespace TestFramework
{
    public class DataRow : Dictionary<string, double> {}

    public enum Operator
    {
        Lower,
        Greater,
        Equals,
        MinMax,
        ValueSet,
        /*
            Analyze_* is a placeholder for now
        */
        Analyze_Lower,
        Analyze_Greater,
        Analyze_Equals,
        Analyze_MinMax,
        Analyze_ValueSet
    }

    public enum SegmentType
    {
        Distance,
        Time
    }

    public static class ModFileHeader
    {   
        public static string    time               = "time [s]";
        public static string    dt                 = "dt [s]";
        public static string    dist               = "dist [m]";
        public static string    v_act              = "v_act [km/h]";
        public static string    v_targ             = "v_targ [km/h]";
        public static string    acc                = "acc [m/s^2]";
        public static string    grad               = "grad [%]";
        public static string    Gear               = "Gear [-]";
        public static string    TC_locked          = "TC locked";
        public static string    n_eng_avg          = "n_eng_avg [1/min]";
        public static string    T_eng_fcmap        = "T_eng_fcmap [Nm]";
        public static string    Tq_full            = "Tq_full [Nm]";
        public static string    Tq_drag            = "Tq_drag [Nm]";
        public static string    P_eng_fcmap        = "P_eng_fcmap [kW]";
        public static string    P_eng_full         = "P_eng_full [kW]";
        public static string    P_eng_full_stat    = "P_eng_full_stat [kW]";
        public static string    P_eng_drag         = "P_eng_drag [kW]";
        public static string    P_eng_inertia      = "P_eng_inertia [kW]";
        public static string    P_eng_out          = "P_eng_out [kW]";
        public static string    P_gbx_shift_loss   = "P_gbx_shift_loss [kW]";
        public static string    P_TC_loss          = "P_TC_loss [kW]";
        public static string    P_TC_out           = "P_TC_out [kW]";
        public static string    P_aux              = "P_aux [kW]";
        public static string    P_gbx_in           = "P_gbx_in [kW]";
        public static string    P_gbx_loss         = "P_gbx_loss [kW]";
        public static string    P_gbx_inertia      = "P_gbx_inertia [kW]";
        public static string    P_ret_in           = "P_ret_in [kW]";
        public static string    P_ret_loss         = "P_ret_loss [kW]";
        public static string    P_angle_in         = "P_angle_in [kW]";
        public static string    P_angle_loss       = "P_angle_loss [kW]";
        public static string    P_axle_in          = "P_axle_in [kW]";
        public static string    P_axle_loss        = "P_axle_loss [kW]";
        public static string    P_brake_in         = "P_brake_in [kW]";
        public static string    P_brake_loss       = "P_brake_loss [kW]";
        public static string    P_wheel_in         = "P_wheel_in [kW]";
        public static string    P_wheel_inertia    = "P_wheel_inertia [kW]";
        public static string    P_trac             = "P_trac [kW]";
        public static string    P_slope            = "P_slope [kW]";
        public static string    P_air              = "P_air [kW]";
        public static string    P_roll             = "P_roll [kW]";
        public static string    P_veh_inertia      = "P_veh_inertia [kW]";
        public static string    n_gbx_out_avg      = "n_gbx_out_avg [1/min]";
        public static string    T_gbx_out          = "T_gbx_out [Nm]";
        public static string    TCnu               = "TCnu";
        public static string    TCmu               = "TCmu";
        public static string    T_TC_out           = "T_TC_out";
        public static string    n_TC_out           = "n_TC_out";
        public static string    T_TC_in            = "T_TC_in";
        public static string    n_TC_in            = "n_TC_in";
        public static string    P_aux_CYCLE        = "P_aux_CYCLE";
        public static string    P_aux_CONSTANTAUX  = "P_aux_CONSTANTAUX";
        public static string    FC_Map             = "FC-Map [g/h]";
        public static string    FC_NCVc            = "FC-NCVc [g/h]";
        public static string    FC_WHTCc           = "FC-WHTCc [g/h]";
        public static string    FC_AAUX            = "FC-AAUX [g/h]";
        public static string    FC_ADAS            = "FC-ADAS [g/h]";
        public static string    FC_Final           = "FC-Final [g/h]";
    }
}
