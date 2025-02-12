import os

def print_directory_structure(root_dir, ignored_dirs=None):
    if ignored_dirs is None:
        ignored_dirs = []  # Default to an empty list if no dirs are specified to ignore

    for dirpath, dirnames, filenames in os.walk(root_dir):
        # Skip ignored directories
        dirnames[:] = [d for d in dirnames if d not in ignored_dirs]
        
        # Indentation based on directory depth
        depth = dirpath.count(os.sep) - root_dir.count(os.sep)
        indent = ' ' * 4 * depth
        print(f"{indent}{os.path.basename(dirpath)}/")
        
        # Print filenames
        for filename in filenames:
            print(f"{indent}    {filename}")

# Automatically get the current directory where the script is located
root_directory = os.getcwd()

# List of folders to ignore (e.g., "bin", "obj", "test")
ignored_folders = ["bin", "obj", "test", ".git"]  # Change these to the folder names you want to ignore

# Call the function
print_directory_structure(root_directory, ignored_dirs=ignored_folders)
