using System.Collections.Generic;
using CsvHelper;
using System;
using System.IO;
using System.Data;

namespace TestFramework
{
    public class ModFileData
    {
        private class DataLine
        {
            public double    time               { get; set; }
            public double    dt                 { get; set; }
            public double    dist               { get; set; }
            public double    v_act              { get; set; }
            public double    v_targ             { get; set; }
            public double    acc                { get; set; }
            public double    grad               { get; set; }
            public double    Gear               { get; set; }
            public double    TC_locked          { get; set; }
            public double    n_eng_avg          { get; set; }
            public double    T_eng_fcmap        { get; set; }
            public double    Tq_full            { get; set; }
            public double    Tq_drag            { get; set; }
            public double    P_eng_fcmap        { get; set; }
            public double    P_eng_full         { get; set; }
            public double    P_eng_full_stat    { get; set; }
            public double    P_eng_drag         { get; set; }
            public double    P_eng_inertia      { get; set; }
            public double    P_eng_out          { get; set; }
            public double    P_gbx_shift_loss   { get; set; }
            public double    P_TC_loss          { get; set; }
            public double    P_TC_out           { get; set; }
            public double    P_aux              { get; set; }
            public double    P_gbx_in           { get; set; }
            public double    P_gbx_loss         { get; set; }
            public double    P_gbx_inertia      { get; set; }
            public double    P_ret_in           { get; set; }
            public double    P_ret_loss         { get; set; }
            public double    P_angle_in         { get; set; }
            public double    P_angle_loss       { get; set; }
            public double    P_axle_in          { get; set; }
            public double    P_axle_loss        { get; set; }
            public double    P_brake_in         { get; set; }
            public double    P_brake_loss       { get; set; }
            public double    P_wheel_in         { get; set; }
            public double    P_wheel_inertia    { get; set; }
            public double    P_trac             { get; set; }
            public double    P_slope            { get; set; }
            public double    P_air              { get; set; }
            public double    P_roll             { get; set; }
            public double    P_veh_inertia      { get; set; }
            public double    n_gbx_out_avg      { get; set; }
            public double    T_gbx_out          { get; set; }
            public double    TCnu               { get; set; }
            public double    TCmu               { get; set; }
            public double    T_TC_out           { get; set; }
            public double    n_TC_out           { get; set; }
            public double    T_TC_in            { get; set; }
            public double    n_TC_in            { get; set; }
            public double    P_aux_CYCLE        { get; set; }
            public double    P_aux_CONSTANTAUX  { get; set; }
            public double    FC_Map             { get; set; }
            public double    FC_NCVc            { get; set; }
            public double    FC_WHTCc           { get; set; }
            public double    FC_AAUX            { get; set; }
            public double    FC_ADAS            { get; set; }
            public double    FC_Final           { get; set; }
        }
        
        private DataTable m_data_table;

        public ModFileData(string path_to_modfile)
        {

            m_data_table = new DataTable();

            m_data_table.Columns.Add(ModFileHeader.time             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.dt               , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.dist             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.v_act            , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.v_targ           , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.acc              , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.grad             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.Gear             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.TC_locked        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.n_eng_avg        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.T_eng_fcmap      , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.Tq_full          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.Tq_drag          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_fcmap      , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_full       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_full_stat  , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_drag       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_inertia    , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_eng_out        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_gbx_shift_loss , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_TC_loss        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_TC_out         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_aux            , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_gbx_in         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_gbx_loss       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_gbx_inertia    , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_ret_in         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_ret_loss       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_angle_in       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_angle_loss     , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_axle_in        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_axle_loss      , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_brake_in       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_brake_loss     , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_wheel_in       , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_wheel_inertia  , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_trac           , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_slope          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_air            , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_roll           , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_veh_inertia    , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.n_gbx_out_avg    , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.T_gbx_out        , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.TCnu             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.TCmu             , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.T_TC_out         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.n_TC_out         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.T_TC_in          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.n_TC_in          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_aux_CYCLE      , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.P_aux_CONSTANTAUX, typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_Map           , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_NCVc          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_WHTCc         , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_AAUX          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_ADAS          , typeof(double));
            m_data_table.Columns.Add(ModFileHeader.FC_Final         , typeof(double));

            using (var reader = new StreamReader(path_to_modfile))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    Console.WriteLine("Started reading modfile at: " + path_to_modfile);
                    // var records = csv.GetRecords<DataLine>();

                    m_data_table.Load(dr);
                    
                    foreach(DataRow dataRow in m_data_table.Rows)
                    {
                        // Console.WriteLine();
                        foreach(var item in dataRow.ItemArray)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
            }
        }
    }
}