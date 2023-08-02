set GEN_CLIENT=.\Luban.ClientServer\Luban.ClientServer.exe

set INPUT_XML=.\Configs\Datas
set OUTPUT_GEN=Configs\Gen
set OUTPUT_JSON=Configs\output_json

%GEN_CLIENT% -j cfg --^
 -d .\Configs\Defines\__root__.xml ^
 --input_data_dir %INPUT_XML% ^
 --output_code_dir %OUTPUT_GEN% ^
 --output_data_dir %OUTPUT_JSON% ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

pause