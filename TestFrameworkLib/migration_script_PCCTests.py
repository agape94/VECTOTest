#!/usr/bin/python

import sys
import os
import re

assert len(sys.argv) > 1

input_file_path = os.getcwd() + "/" + sys.argv[1]

time_tolerance = 0
if len(sys.argv) > 2:
    time_tolerance = float(sys.argv[2])

print(f"Converting file: {input_file_path}")

f_in = open(input_file_path, "rt")
lines = f_in.readlines()

input_file_name = input_file_path[input_file_path.rfind("/")+1:]
output_file_name = input_file_name[:input_file_name.rfind(".cs")] + "_migrated" + ".cs"
output_file_path = input_file_path[:input_file_path.rfind("/")+1] + output_file_name
f_out = open(output_file_path, "w")

migrated_lines = []
migrated_lines.append("using TUGraz.VectoCore.Tests.TestFramework;\n")

pattern = r"\t\t\t\(.*\,.*\,.*\,.*\).*"

last_end = -100

for line in lines:
    if line.find("public class") != -1:
        last_space_pos = line.rfind(" ")
        class_name = line[last_space_pos + 1:-1]
        class_name += "_Migrated : VECTOTest\n"
        line = line[:last_space_pos] + " " + class_name
        migrated_lines.append(line)
    elif line.find("=> TestPCC") != -1:
        test_function_line = line[:line.rfind("=> TestPCC")] + "=> RunTestCases(MethodBase.GetCurrentMethod().Name,\n"
        migrated_lines.append(test_function_line)
    elif line.find("private const string BasePath") != -1:
        base_path_string = line[line.rfind("@"):line.rfind(";")]
        line = line[:line.find("private")] + "public override string BasePath { get => " + base_path_string + "; }\n"
        migrated_lines.append(line)
    elif re.match(pattern, line):
        open_paranthesis_pos = line.find("(")

        pcc_states_line = line[:open_paranthesis_pos]
        driver_action_line = line[:open_paranthesis_pos]
        
        comma_pos = line.find(",")
        comma_pos_2 = line.find(",", comma_pos + 1)

        start = line[open_paranthesis_pos + 1 : comma_pos]
        end = line[comma_pos + 1 : comma_pos_2]

        comma_pos = line.find(",", comma_pos_2 + 1)
        pcc_state = line[comma_pos_2 + 1 : comma_pos]

        close_paranthesis_pos = line.find(")", comma_pos + 1)
        driver_action = line[comma_pos + 1 : close_paranthesis_pos]

        start = start.replace(" ", "")
        end = end.replace(" ", "")
        pcc_state = pcc_state.replace(" ", "")
        driver_action = driver_action.replace(" ", "")

        time_tolerance_string = ", " + str(time_tolerance) if time_tolerance != 0 else ""

        pcc_states_line += "TC(" + start + ", " + end + time_tolerance_string + ", " + "\"PCCState\"" + ", " + "Operator.Equals" + ", " + "(int) " + pcc_state + ")"
        driver_action_line += "TC(" + start + ", " + end + time_tolerance_string + ", " + "\"DriverAction\"" + ", " + "Operator.Equals" + ", " + "(int) " + driver_action + ")"
        
        pcc_states_line += ",\n"
        if(line[line.rfind(")") + 1] == ";"):
            driver_action_line += "\n\t\t);"
        else:
            driver_action_line += ",\n"
        
        line = line[:open_paranthesis_pos] + "// " + line[open_paranthesis_pos:]
        migrated_lines.append(line)
        migrated_lines.append(pcc_states_line)
        migrated_lines.append(driver_action_line)
    else:
        migrated_lines.append(line)

migrated_file_string = ''.join(migrated_lines)

f_out.write(migrated_file_string)
f_in.close()
f_out.close()
print("Done converting test class!")
print(f"Converted file: {output_file_path}")