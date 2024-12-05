import subprocess
import pandas as pd
import matplotlib.pyplot as plt

class DemographicSimulator:
    def __init__(self, engine_path: str):
        self.engine_path = engine_path
    

    def run_simulation(self, initial_population_file, death_rates_file, start_year, end_year, initial_population):
        args = [
            self.engine_path,
            initial_population_file,
            death_rates_file,
            str(start_year),
            str(end_year),
            str(initial_population)
        ]

        try:
            # Запуск C# процесса
            subprocess.run(args, check=True)
            print(f"Запрос субпроцесса выполнен")
        except subprocess.CalledProcessError as e:
            print(f"Error during simulation: {e}")
            return None

        # Загрузка результатов моделирования
        output_file = "/Users/nikitavolnuhin/Labs_cs/lab6/SimulationResults.csv"
        return pd.read_csv(output_file)

    @staticmethod
    def visualize_results(results):
        # График изменения общего населения
        plt.figure(figsize=(12, 6))
        plt.plot(results['Year'], results['TotalPopulation'], label='Общее население', color='blue', linestyle='-', marker='o')
        plt.plot(results['Year'], results['MalePopulation'], label='Мужчины', color='green', linestyle='--', marker='x')
        plt.plot(results['Year'], results['FemalePopulation'], label='Женщины', color='red', linestyle='-.', marker='s')
        plt.title("Изменение численности населения по годам")
        plt.xlabel("Год")
        plt.ylabel("Население (тыс. чел.)")
        plt.legend()
        plt.grid(True)
        plt.show()

        # Бар-чарт возрастных групп
        last_year = results['Year'].iloc[-1]
        male_ages = {'0-18': 10_000, '19-45': 12_000, '45-65': 9_000, '65-100': 7_000}  # Заглушка
        female_ages = {'0-18': 10_500, '19-45': 12_500, '45-65': 8_500, '65-100': 6_500}  # Заглушка

        plt.figure(figsize=(12, 6))
        plt.bar(male_ages.keys(), male_ages.values(), alpha=0.7, label='Мужчины', color='green')
        plt.bar(female_ages.keys(), female_ages.values(), alpha=0.7, label='Женщины', color='red')
        plt.title(f"Возрастной состав населения на конец {last_year} года")
        plt.ylabel("Население (тыс. чел.)")
        plt.legend()
        plt.grid(axis='y')
        plt.show()
