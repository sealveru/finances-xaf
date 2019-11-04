import os, shutil
from pathlib import Path
from distutils.dir_util import copy_tree
from dotenv import load_dotenv


env_path = Path('.') / '.env'
load_dotenv(dotenv_path=env_path)
tests_executor = os.getenv("EASY_TEST_EXECUTOR_PATH")
vs_path =  os.getenv("VISUAL_STUDIO_PATH")
msbuild = f'{vs_path}\MSBuild\\Current\Bin\MSBuild.exe'
mstest = f'{vs_path}\Common7\IDE\MSTest.exe'
    
src_folder = 'src'
artifacts_folder = 'artifacts'
tests_folder = 'tests'
tests_result_folder = f'{tests_folder}\\TestResult'
functional_tests_result_folder = f'{tests_result_folder}\\FunctionalTests'
exts_to_clear = ['pdb', 'xml']
build_modes = ['Release', 'EasyTest']


def print_title(title):
    print('\n\n\n' + ('*' * 60))
    print(title)
    print(('*' * 60) + '\n')
    

def delete_elements(paths):
    for path in paths:
        print(f'ELIMINANDO: {path}')

        if os.path.isfile(path):
            os.unlink(path)
        elif os.path.isdir(path): 
            shutil.rmtree(path)


def clear_solutions():
    folders = []
    folders.extend(Path(f'{src_folder}').glob(f'**/bin/*'))
    folders.extend(Path(f'{tests_folder}').glob(f'**/bin/*'))
    folders.extend(Path(f'{tests_result_folder}').glob('*'))
    folders.extend(Path(f'{artifacts_folder}').glob('*'))

    delete_elements(folders)


def build_solutions(mode):
    solutions = list(Path(f'{src_folder}').glob(f'**/*.sln'))
    for solution in solutions:
        print(f'\n----- COMPILANDO: {solution} EN MODO: {mode} \n')
        command = (
            f'"{msbuild}" '
            f'{solution} '
            f'/t:Build '
            f'/p:BuildProjectReferences=true '
            f'-v:minimal '
            f'-p:Configuration={mode} '
            f'/p:OutputPath=bin/{mode} '
        )
        os.system(command)


def get_tests_info():
    result = []
    projects = os.listdir(tests_folder)

    for project in projects:
        root = f'{tests_folder}\\{project}'
        settings = list(Path(root).glob('**/*.testsettings'))

        if len(settings) > 0:
            result.append({
                'root': root,
                'name': project,
                'dll': f'{root}\\bin\\Release\\{project}.dll',
                'settings': str(settings[0])
            })

    return result


def run_unit_tests():
    tests_info = get_tests_info()
    for project in tests_info:
        command = (
            f'"{mstest}" '
            f'/testcontainer:{project["dll"]} '
            f'/resultsfile:{tests_result_folder}\\{project["name"]}.trx '
            f'/testsettings:{project["settings"]}'
        )
        os.system(command)


def run_functional_tests():
    folders = []
    folders.extend(Path(f'{src_folder}').glob(f'**/FunctionalTests'))
    folders.extend(Path(f'{tests_folder}').glob(f'**/FunctionalTests'))

    for folder in folders:
        print(f'COPIANDO: {folder}')
        copy_tree(folder, functional_tests_result_folder)

    os.system(f'"{tests_executor}" {functional_tests_result_folder}')


def copy_artifacts():
    projects = os.listdir(src_folder)
    for project in projects:
        if 'DevSol' in project and not '.sln' in project:
            root = os.path.join(src_folder, project)
            source = f'{root}\\bin\Release'
            target = f'{artifacts_folder}\\{project}'
            print(f'COPIANDO: {target}')
            copy_tree(source, target)
    
    print('\n----- ELIMINANDO ARCHIVOS INNECESARIOS \n')
    for ext in exts_to_clear:
        delete_elements(list(Path(f'{artifacts_folder}').glob(f'**/*.{ext}')))


def run():
    print_title('LIMPIANDO SOLUCIONES')
    clear_solutions()

    print_title('COMPILANDO SOLUCIONES')
    for mode in build_modes:
        build_solutions(mode)

    print_title('EJECUTANDO PRUEBAS UNITARIAS')
    run_unit_tests()

    print_title('EJECUTANDO PRUEBAS FUNCIONALES')
    run_functional_tests()

    print_title('COPIANDO ARTEFACTOS')
    copy_artifacts()


run()
