from trt_llama_api import TrtLlmAPI #llama_index does not currently support TRT-LLM. The trt_llama_api.py file defines a llama_index compatible interface for TRT-LLM.
from llama_index.llms.llama_utils import messages_to_prompt, completion_to_prompt
from llama_index import set_global_service_context
import sys
import os

model_directory = ".\\model"
tokenizer_directory = ".\\tokenizer"
system_message = "You are a story teller assistant. Give a description of a scene. The topic will be given by the user."

files = os.listdir(model_directory)

engine_file = [file for file in files if file.endswith(".engine")]

# create trt_llm engine object
llm = TrtLlmAPI(
    model_path=model_directory,
    engine_name=engine_file[0],
    tokenizer_dir= tokenizer_directory,
    temperature=5,
    max_new_tokens=1024,
    context_window=3900,
    messages_to_prompt=messages_to_prompt,
    completion_to_prompt=completion_to_prompt,
    verbose=False
)


def remove_substrings(main_string, substrings_to_remove):
    for substring in substrings_to_remove:
        main_string = main_string.replace(substring, '')
    return main_string

def main():
    while True:
        user_input = input("Enter text (type 'exit' to quit): ")
        
        if user_input.lower() == "exit":
            break
        
        response = llm.complete(completion_to_prompt(user_input, system_message), formatted=True)

        response = remove_substrings(response.text, ["</s>", "\"", "Narrator:", "Bot:"])

        print()
        print("Bot:", response)
        print()


if __name__ == "__main__":
    main()