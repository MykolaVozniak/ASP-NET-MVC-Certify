selected_directory <- "C:/Users/mvozn/Desktop/R/quantquote_select"

# Функція для обчислення log-returns
calculate_log_returns <- function(data) {
  log_returns <- log(data$clo[-nrow(data)] / data$clo[-1])
  return(log_returns)
}

# Функція для обчислення логарифмів відношення mx/mn
calculate_mr <- function(data) {
  mr <- log(data$mx / data$mn)
  return(mr)
}

# Функція для обчислення логарифмів відношення opn/clo
calculate_or <- function(data) {
  or <- log(data$opn / data$clo)
  return(or)
}

# Читаємо файли компаній і виконуємо розрахунки
for (symbol in c("altr", "amat", "amd")) {
  file_name <- paste0("table_", symbol, ".csv")
  file_path <- file.path(selected_directory, file_name)
  data <- read.csv(file_path, header = FALSE)
  colnames(data) <- c("dat", "z", "opn", "mx", "mn", "clo", "vol")
  
  lr <- calculate_log_returns(data)
  mr <- calculate_mr(data)
  or <- calculate_or(data)
  
  # Виводимо результати
  print(paste("Company:", symbol))
  print("Log-returns (lr):")
  print(lr)
  print("Logarithm of mx/mn (mr):")
  print(mr)
  print("Logarithm of opn/clo (or):")
  print(or)
  print("")
}