from trt_llama_api import TrtLlmAPI #llama_index does not currently support TRT-LLM. The trt_llama_api.py file defines a llama_index compatible interface for TRT-LLM.
from llama_index.llms.llama_utils import messages_to_prompt, completion_to_prompt
from llama_index import set_global_service_context
import sys
import os
import socket
from gtts import gTTS
from pydub import AudioSegment
from mutagen.mp3 import MP3
import pyaudio
import signal
import socket
import sys
import select
import random

host, port = "127.0.0.1", 25001
# SOCK_STREAM means TCP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

model_directory = ".\\model"
tokenizer_directory = ".\\tokenizer"

# I could have used just one prompt, but I wanted a variaty of responses for the demo (that were actually testable). Having a lower temperature with this gives the effect I wanted but this could easily be messed around with.
system_message1 = "User messages will be ideas. Make fun of these ideas as if you were narrating. Do not reffer to yourself. Keep your responses short."
system_message2 = "You are an ai in charge of keeping the room tidy. Keep your responses short, one to two sentances. You get annoyed when people ruin stuff or act crazy. Do not reffer to yourself."

system_message = [system_message1, system_message2]

files = os.listdir(model_directory)

engine_file = [file for file in files if file.endswith(".engine")]

# create trt_llm engine object
llm = TrtLlmAPI(
    model_path=model_directory,
    engine_name=engine_file[0],
    tokenizer_dir= tokenizer_directory,
    temperature=.1,
    max_new_tokens=1024,
    context_window=3900,
    messages_to_prompt=messages_to_prompt,
    completion_to_prompt=completion_to_prompt,
    verbose=False
)

def play_mp3(file_path):
    audio = AudioSegment.from_mp3(file_path)

    # Convert to raw PCM audio data
    sample_rate = audio.frame_rate
    sample_width = audio.sample_width
    channels = audio.channels
    raw_data = audio.raw_data

    p = pyaudio.PyAudio()
    stream = p.open(format=p.get_format_from_width(sample_width),
                    channels=channels,
                    rate=sample_rate,
                    output=True)

    stream.write(raw_data)
    stream.stop_stream()
    stream.close()
    p.terminate()

def get_audio_length(file_path):
    audio = MP3(file_path)
    return audio.info.length

def send_message(message):
    try:
        # Send the data
        sock.sendall(message.encode("utf-8"))
    except:
        print("unknown error occurred")

def recieve_message():
    try:
        # Timeout of 2 secs, recieves message from unity
        ready = select.select([sock], [], [], 2)
        while not ready[0]:
            ready = select.select([sock], [], [], 2)
        
        return sock.recv(1024).decode("utf-8")
    except KeyboardInterrupt:
        print("unknown error occurred")

# Used to remove little bits of text that the llm likes to use but don't make sense
def remove_substrings(main_string, substrings_to_remove):
    for substring in substrings_to_remove:
        main_string = main_string.replace(substring, '')
    return main_string

def main():
    while True:
        print("Waiting for message from game...")
        print()

        user_input = recieve_message()
        
        print("Recieved: " + user_input)
        print()

        if user_input.lower() == "exit":
            break

        system_prompt = random.choice(system_message)

        response = llm.complete(completion_to_prompt(user_input, system_prompt), formatted=True)
        print()

        # Add any strings that pop up and are undesired
        response = remove_substrings(response.text, ["</s>", "\"", "Narrator:", "Bot:"])

        tts = gTTS(response, lang='en', tld='com.au')
        tts.save('output.mp3')

        # Concat the length of the audio clip onto the message we send to unity. 
        # Lets unity time the display of subtitles a bit better. Janky but works
        response = str(get_audio_length("output.mp3")) + " " + response
            
        print("Sending: " + response)
        print()
        send_message(response)

        play_mp3("output.mp3")        

# Define a signal handler for SIGINT (Ctrl+C)
def signal_handler(sig, frame):
    sock.close()
    sys.exit(0)

if __name__ == "__main__":
    signal.signal(signal.SIGINT, signal_handler)
    main()






