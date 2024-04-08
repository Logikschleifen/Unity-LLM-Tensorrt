# Unity-LLM-Tensorrt
A simple demo game made in unity to demonstrate my idea for using AI in video games (at least until gpt 6.7 comes out). Made for the nividia dev competition.

Youtube vid (Im not great at making these so plz be kind) - https://www.youtube.com/watch?v=hnojD7O0nKk&t=114s 
## Python Setup
Prerequisites are the same as [tensorrt-llm for windows](https://github.com/NVIDIA/TensorRT-LLM/blob/main/windows/README.md): 
- [Python 3.10](https://www.python.org/downloads/windows/)
- [CUDA 12.2 Toolkit](https://developer.nvidia.com/cuda-12-2-2-download-archive?target_os=Windows&target_arch=x86_64)
- [Microsoft MPI](https://www.microsoft.com/en-us/download/details.aspx?id=57467)
- [cuDNN](https://developer.nvidia.com/cudnn)

I would strongly recommend also getting anaconda and setting up a environment using python 3.10.

### Steps: 
1. Clone this repository.

2. If using anaconda than create and activate a environment using python 3.10

3. Run a terminal in: Unity-LLM-Tensorrt/Python 

4. Run this command:

```
pip install -r requirments.txt
```

5. Then this one:

```
pip install tensorrt_llm --extra-index-url https://pypi.nvidia.com --extra-index-url https://download.pytorch.org/whl/cu121
```

6. In order to run the program you will need a .engine file for the model you would like to use as well as it's corresponding tokenizer. Follow instructions from [trt-llm-rag-windows](https://github.com/NVIDIA/trt-llm-rag-windows) for building a TRT engine if you don't have one.
   Once acquired put files into the Unity-LLM-Tensorrt/Python/model and Unity-LLM-Tensorrt/Python/model according to each folder's readme.

7. Your done! To run the program just use:
```
python ./app.py
```
Note: I made a fun terminal chatbot with no conversation memory as well, to run it use:
```
python ./chat_no_mem.py
```
Use ctrl-c to exit either program.

## Unity Setup
Required version: 
* Unity 2022.3.14

### Steps: 
1. Install [unity](https://unity.com/unity-hub)

2. Open unity hub
   
3. Click on the Add button
   
4. Find the project folder in Unity-LLM-Tensorrt/Unity/AInGames
   
5. Should be able to launch the project and play it like any other unity project. There is only one scene in scenes folder called "main", thats the one.

Note: Unity and git aren't exactly the best combo. If any issues happen than please let me know.

## Use
Once you have the python and unity sides working try running them. If you are running in the unity editor, click the play button at the top. Then go activate the python code as the game is running. Allow the programs to use local network (nessesary for thier communication). If you are running in the unity editor you should see text appear in the console
indicating communication between the programs. W, A, S, D to move, space to jump, and left click to pick up. Have fun, here are the 4 types of triggers I added.

1. Picking up objects has a 1 in 3 chance to trigger a response (100 percent was really annoying)
2. Throwing objects into a wall hard enough.
3. Jumping off tables.
4. Staring at wall, ceiling, or floor for 3 seconds.

## Resources
* I used nividia's [trt-llm-rag-windows](https://github.com/NVIDIA/trt-llm-rag-windows) to
get started on the project.
* I used [this](https://assetstore.unity.com/packages/3d/environments/sci-fi/free-sci-fi-office-pack-195067) asset pack for the room.
* And I used [this](https://assetstore.unity.com/packages/3d/props/interior/kitchen-props-free-80208) asset pack for the dishes.
