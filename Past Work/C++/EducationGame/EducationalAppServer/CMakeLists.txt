cmake_minimum_required(VERSION 3.2)
project(EducationalAppServer)

set(MONGOOSE_SHIT mongoose-cpp)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")
set(SOURCE_FILES main.cpp webs.cpp webs.h)
set(EXECUTABLE_NAME "EducationalAppServer")

include_directories(${MONGOOSE_SHIT})
include_directories(${MONGOOSE_SHIT}/mongoose)

link_directories(${MONGOOSE_SHIT}/mongoose)
link_directories(${MONGOOSE_SHIT})

add_executable(${EXECUTABLE_NAME} ${SOURCE_FILES})

target_link_libraries(${EXECUTABLE_NAME} mongoose)

set(CMAKE_MODULE_PATH "${CMAKE_SOURCE_DIR}/cmake_modules" ${CMAKE_MODULE_PATH})

set(THREADS_PREFER_PTHREAD_FLAG ON)
find_package(Threads REQUIRED)
target_link_libraries(${EXECUTABLE_NAME} Threads::Threads)


find_package(SFML 2 REQUIRED system network)
if(SFML_FOUND)
    include_directories(${SFML_INCLUDE_DIR})
    target_link_libraries(${EXECUTABLE_NAME} ${SFML_LIBRARIES})
endif()

find_package(MySQLConnectorCPP)
if(MYSQLCONNECTORCPP_FOUND)
    include_directories(${MYSQLCONNECTORCPP_INCLUDE_DIR})
    target_link_libraries(${EXECUTABLE_NAME} ${MYSQLCONNECTORCPP_LIBRARY})
endif()

add_custom_command(TARGET EducationalAppServer PRE_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
        ${CMAKE_SOURCE_DIR}/www $<TARGET_FILE_DIR:EducationalAppServer>/www)

add_custom_command(TARGET EducationalAppServer PRE_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
        ${CMAKE_SOURCE_DIR}/${MONGOOSE_SHIT} $<TARGET_FILE_DIR:EducationalAppServer>/${MONGOOSE_SHIT})